using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Rate;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleFakeRent.Extensions;

namespace SimpleFakeRent.Controllers
{
    [Route("api/rate")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public RateController(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        //GET api/rate
        [HttpGet(Name = "Rates")]
        public async Task<IActionResult> GetAllRates()
        {
            try
            {
                var rates = await _repositoryWrapper.Rate.GetAllRates();

                if (rates == null)
                {
                    _logger.LogError($"Rates have not been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned all rows from table Rate.");
                    var ratesResult = _mapper.Map<IEnumerable<RateDto>>(rates);
                    return Ok(ratesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAllRates() action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rate/{rate_id}
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}", Name = "RateById")]
        public async Task<IActionResult> GetRateById(int id)
        {
            try
            {
                var rate = await _repositoryWrapper.Rate.GetRateById(id);

                if (rate == null)
                {
                    _logger.LogError($"Rate with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rate with id: {id}");
                    var rateResult = _mapper.Map<RateDto>(rate);
                    return Ok(rateResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRateById(rate_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rate/property/{property_id}
        [Authorize(Policy = "Landlord")]
        [HttpGet("property/{id}", Name = "RateByPropertyId")]
        public async Task<IActionResult> GetRateByPropertyId(int id)
        {
            try
            {
                var rate = await _repositoryWrapper.Rate.GetRateByPropertyId(id);

                if (rate == null)
                {
                    _logger.LogError($"Rate with PropertyId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rate with PropertyId: {id}");
                    var rateResult = _mapper.Map<RateDto>(rate);
                    return Ok(rateResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRateByPropertyId(property_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }


        //GET api/rate/{id}/property
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/property")]
        public async Task<IActionResult> GetRateWithPropertyByRateId(int id)
        {
            try
            {
                var rate = await _repositoryWrapper.Rate.GetRateWithProperty(id);

                if (rate == null)
                {
                    _logger.LogError($"Rate with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rate with id: {id} with related property.");
                    var landlordResult = _mapper.Map<RateWithPropertyDto>(rate);
                    return Ok(landlordResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRateWithProperty(rate_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get rate with related property and address by property_id
        /// </summary>
        /// <param name="id">property_id</param>
        /// <returns>rate with property</returns>
        //GET api/rate/{id}/property/address
        [Authorize(Policy = "Landlord")]
        [HttpGet("property/{id}/property/address")]
        public async Task<IActionResult> GetRateWithPropertyAndAddressByPropertyId(int id)
        {
            try
            {
                bool userOwnsRate = await _repositoryWrapper.Property.CheckIfUserOwnsProperty(HttpContext.SelectLoggedUserId(), id);

                if (!userOwnsRate)
                {
                    return Unauthorized();
                }
                
                var rate = await _repositoryWrapper.Rate.GetRateWithPropertyAndAddressByPropertyId(id);

                if (rate == null)
                {
                    _logger.LogError($"Rate with property_id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rate by property_id: {id} with related property and address.");
                    var landlordResult = _mapper.Map<RateWithPropertyAndAddressDto>(rate);
                    return Ok(landlordResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRateWithPropertyAndAddressByPropertyId(rate_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //POST api/rate
        [Authorize(Policy = "Landlord")]
        [HttpPost]
        public async Task<IActionResult> CreateRate([FromBody] RateForCreationDto rate)
        {
            try
            {

                if (rate == null)
                {
                    _logger.LogError("Rate received is a Null Object.");
                    return BadRequest("Rate object is null. Please send full request.");
                }
                else if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Rate object sent from client.");
                    return BadRequest("Rate object is not Valid");
                }

                Boolean propertyExist = await _repositoryWrapper.Property.CheckIfPropertyExistByPropertyId(rate.PropertyId);
                if (!propertyExist)
                {
                    _logger.LogError($"User tried to create Rate with related property_id: {rate.PropertyId} which does not exist in DB.");
                    return NotFound($"Related Property with id: {rate.PropertyId} does not exist.");
                }
                
                var rateAlreadyInDB = await _repositoryWrapper.Rate.GetRateByPropertyId(rate.PropertyId);
                if (rateAlreadyInDB != null)
                {
                    _logger.LogError("Rate with given PropertyId already exist in Db, cannot create.");
                    return BadRequest("Rate already in Database");
                }

                var rateEntity = _mapper.Map<Rate>(rate);

                _repositoryWrapper.Rate.CreateRate(rateEntity);
                await _repositoryWrapper.Save();

                var createdRate = _mapper.Map<RateDto>(rateEntity);
                return CreatedAtRoute("RateById", new { id = createdRate.Id }, createdRate);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside CreateRate(RateForCreationDto) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //PUT api/rate
        [Authorize(Policy = "Landlord")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRate(int id, [FromBody]RateForUpdateDto rate)
        {
            try
            {
                bool userOwnsRate = await _repositoryWrapper.Property.CheckIfUserOwnsProperty(HttpContext.SelectLoggedUserId(), rate.PropertyId);
                if (!userOwnsRate)
                {
                    return Unauthorized();
                }

                if (rate == null)
                {
                    _logger.LogError("Rate received is a Null Object.");
                    return BadRequest("Rate object is null. Please send full request.");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Rate object sent from client.");
                    return BadRequest("Rate object is not Valid");
                }

                Boolean propertyExist = await _repositoryWrapper.Property.CheckIfPropertyExistByPropertyId(rate.PropertyId);
                if (!propertyExist)
                {
                    _logger.LogError($"User tried to update Rate with related property_id: {rate.PropertyId} which does not exist in DB.");
                    return NotFound($"Related Property with id: {rate.PropertyId} does not exist.");
                }

                var rateEntityFound = await _repositoryWrapper.Rate.GetRateById(id);
                if (rateEntityFound == null)
                {
                    _logger.LogError($"Rate with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(rate, rateEntityFound);
                _repositoryWrapper.Rate.UpdateRate(rateEntityFound);

                await _repositoryWrapper.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside UpdateRate(id, RateForUpdateDto) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //DELETE api/rate
        [Authorize(Policy = "Landlord")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRate(int id)
        {
            try
            {
                var rate = await _repositoryWrapper.Rate.GetRateById(id);

                if (rate == null)
                {
                    _logger.LogError("Rate Id not found, cannot delete.");
                    return BadRequest("Rate object is null. Please send full request.");
                }

                _repositoryWrapper.Rate.DeleteRate(rate);
                await _repositoryWrapper.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside DeleteRate(rate_id) action: {e.Message}");
                return StatusCode(500, e.ToString());
            }
        }

        //DELETE api/rate
        [Authorize(Policy = "Landlord")]
        [HttpDelete("property/{id}")]
        public async Task<IActionResult> DeleteRateByPropertyId(int id)
        {
            try
            {
                var rate = await _repositoryWrapper.Rate.GetRateByPropertyId(id);

                if (rate == null)
                {
                    _logger.LogError("Rate Id not found, cannot delete.");
                    return BadRequest("Rate object is null. Please send full request.");
                }

                _repositoryWrapper.Rate.DeleteRate(rate);
                await _repositoryWrapper.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside DeleteRateByPropertyId(property_id) action: {e.Message}");
                return StatusCode(500, e.ToString());
            }
        }


    }
}
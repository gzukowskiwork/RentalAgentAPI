using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using SimpleFakeRent.Extensions;
using Contracts;
using System.Linq;
using System.Collections.Generic;
using Entities.DataTransferObjects.Address;
using Microsoft.AspNetCore.Authorization;

namespace SimpleFakeRent.Controllers
{
    [Produces("application/json")]
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public AddressController(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }


        //GET api/address
        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            try
            {
                var addresses = await _repositoryWrapper.Address.GetAllAddresses();
                if (addresses == null)
                {
                    _logger.LogInfo($"No 'Address' has been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned all 'Address' from db. Total number of Addresses found: {addresses.Count()}.");
                    var addressesResult = _mapper.Map<IEnumerable<AddressDto>>(addresses);
                    return Ok(addressesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAllAddresses() action: {e.Message}");
                _logger.LogError(e.ErrorMessageExtension());
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "Landlord")]
        //GET api/address/id
        [HttpGet("{id}", Name = "AddressById")]
        public async Task<IActionResult> GetAdressById(int id)
        {
            try
            {
                var address = await _repositoryWrapper.Address.GetAddressById(id);

                if (address == null)
                {
                    _logger.LogError($"Address with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Address with id: {id}");
                    var addressResult = _mapper.Map<AddressDto>(address);
                    return Ok(addressResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAddressById(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/address/landlord/1
        [Authorize(Policy ="Landlord")]
        [HttpGet("landlord/{id}")]
        public async Task<IActionResult> GetAddressWithLandlord(int id)
        {
            try
            {
                var address = await _repositoryWrapper.Address.GetAddressWithLandlord(id);

                if (address == null)
                {
                    _logger.LogError($"Address with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Address with id: {id} and its Landlord.");
                    var landlordResult = _mapper.Map<AddressWithLandlordDto>(address);
                    return Ok(landlordResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAddressWithLandlord(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/address/property/1
        [Authorize(Policy = "Landlord,Tenant")]

        [HttpGet("property/{id}")]
        public async Task<IActionResult> GetAddressWithProperty(int id)
        {
            try
            {
                var address = await _repositoryWrapper.Address.GetAddressWithProperty(id);

                if (address == null)
                {
                    _logger.LogError($"Address with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Address with id: {id} and its Property.");
                    var landlordResult = _mapper.Map<AddressWithPropertyDto>(address);
                    return Ok(landlordResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAddressWithProperty(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/address/property/1
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("tenant/{id}")]
        public async Task<IActionResult> GetAddressWithTenant(int id)
        {
            try
            {
                var address = await _repositoryWrapper.Address.GetAddressWithTenant(id);

                if (address == null)
                {
                    _logger.LogError($"Address with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Address with id: {id} and its Tenant.");
                    var landlordResult = _mapper.Map<AddressWithTenantDto>(address);
                    return Ok(landlordResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAddressWithTenant(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }


        //GET api/address/city/zakopane
        [Authorize(Policy = "Landlord")]
        [HttpGet("city/{cityName}")]
        public async Task<IActionResult> GetAddressesByCity(string cityName)
        {
            try
            {
                var addresses = await _repositoryWrapper.Address.GetAddressByCity(cityName);

                if (addresses == null)
                {
                    _logger.LogError($"Address with city: {cityName}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Addresses with city: {cityName}, addresses found count: {addresses.Count()}.");
                    var addressesResult = _mapper.Map<IEnumerable<AddressDto>>(addresses);
                    return Ok(addressesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAddressByCity(city) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/address/street/pionkowa
        [Authorize(Policy = "Landlord")]
        [HttpGet("street/{streetName}")]
        public async Task<IActionResult> GetAddressesByStreet(string streetName)
        {
            try
            {
                var addresses = await _repositoryWrapper.Address.GetAddressByStreet(streetName);

                if (addresses == null)
                {
                    _logger.LogError($"Address with Street: {streetName}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Addresses with Street: {streetName}, addresses found count: {addresses.Count()}.");
                    var addressesResult = _mapper.Map<IEnumerable<AddressDto>>(addresses);
                    return Ok(addressesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAddressesByStreet(streetName) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/address/country/polska
        [Authorize(Policy = "Landlord")]
        [HttpGet("country/{countryName}")]
        public async Task<IActionResult> GetAddressByCountry(string countryName)
        {
            try
            {
                var addresses = await _repositoryWrapper.Address.GetAddressByCountry(countryName);

                if (addresses == null)
                {
                    _logger.LogError($"Address with Country: {countryName}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Addresses with Country: {countryName}, addresses found count: {addresses.Count()}.");
                    var addressesResult = _mapper.Map<IEnumerable<AddressDto>>(addresses);
                    return Ok(addressesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAddressByCountry(countryName) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/address/zip/80-765
        [Authorize(Policy = "Landlord")]
        [HttpGet("zip/{postalCode}")]
        public async Task<IActionResult> GetAddressesByZipCode(string postalCode)
        {
            try
            {
                var addresses = await _repositoryWrapper.Address.GetAddressByZipCode(postalCode);

                if (addresses == null)
                {
                    _logger.LogError($"Address with ZipCode: {postalCode}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Addresses with ZipCode: {postalCode}, addresses found count: {addresses.Count()}.");
                    var addressesResult = _mapper.Map<IEnumerable<AddressDto>>(addresses);
                    return Ok(addressesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAddressesByZipCode(ZIP) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }


        //POST api/address
        [Authorize(Policy = "Landlord")]
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressForCreationDto address)
        {
            try
            {
                if (address == null)
                {
                    _logger.LogError("Address received is a Null Object.");
                    return BadRequest("Address object is null. Please send full request.");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Address object sent from client.");
                    return BadRequest("Address object is not Valid");
                }

                var addressEntity = _mapper.Map<Address>(address);

                _repositoryWrapper.Address.CreateAddress(addressEntity);
                await _repositoryWrapper.Save();

                var createdProperty = _mapper.Map<AddressDto>(addressEntity);
                return CreatedAtRoute("AddressById", new { id = createdProperty.Id }, createdProperty);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside CreateAddress(addressDto) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //UPDATE api/address
        [Authorize(Policy = "Landlord")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody]AddressForUpdateDto address)
        {
            try
            {
                //int? landlordId = await _repositoryWrapper.Landlord.FindLandlordIdByAddressId(id);
                //if(HttpContext.SelectLoggedUserId() != landlordId/* && landlordId != null*/)
                //{
                //    return Unauthorized();
                //}
                if (address == null)
                {
                    _logger.LogError("Address received is a Null Object.");
                    return BadRequest("Address object is null. Please send full request.");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Address object sent from client.");
                    return BadRequest("Address object is not Valid");
                }

                var addressEntity = await _repositoryWrapper.Address.GetAddressById(id);

                // Sprawdź czy taki Address istnieje zanim updejtujesz
                if (addressEntity == null)
                {
                    _logger.LogError($"Address with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(address, addressEntity);
                _repositoryWrapper.Address.UpdateAddress(addressEntity);

                await _repositoryWrapper.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside UpdateAddress(id, address) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //DELETE api/address
        [Authorize(Policy = "Landlord")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                var address = await _repositoryWrapper.Address.GetAddressById(id);
                if (address == null)
                {
                    _logger.LogError($"Address with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                Boolean addressUsedByLandlord = await _repositoryWrapper.Landlord.CheckIfLandlordExistByAddressId(address.Id);
                Boolean addressUsedByProperty = await _repositoryWrapper.Property.CheckIfPropertyExistByAddressId(address.Id);
                Boolean addressUsedByTenant = await _repositoryWrapper.Tenant.CheckIfTenantExistByAddressId(address.Id);

                if (addressUsedByLandlord)
                {
                    _logger.LogError($"Address with id: {id}, hasn't is related to Landlord.");
                    return BadRequest("Some Landlord is related to this address and cannot be deleted.");
                }
                else if (addressUsedByProperty)
                {
                    _logger.LogError($"Address with id: {id}, hasn't is related to Property.");
                    return BadRequest("Some Property is related to this address and cannot be deleted.");
                }
                else if (addressUsedByTenant)
                {
                    _logger.LogError($"Address with id: {id}, hasn't is related to Tenant.");
                    return BadRequest("Some Tenant is related to this address and cannot be deleted.");
                }

                _repositoryWrapper.Address.Delete(address);
                await _repositoryWrapper.Save();
                return NoContent();
            }
            catch(Exception e)
            {
                _logger.LogError($"Something went wrong inside DeleteAddress(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

    }
}
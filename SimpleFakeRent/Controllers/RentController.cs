using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Rent;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleFakeRent.Extensions;

namespace SimpleFakeRent.Controllers
{
    [Route("api/rent")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public RentController(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }


        //GET api/rent
        [HttpGet(Name = "Rents")]
        public async Task<IActionResult> GetAllRents()
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetAllRents();

                if (rents == null)
                {
                    _logger.LogError($"Rents, have not been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned all rows from table Rent.");
                    var rentsResult = _mapper.Map<IEnumerable<RentDto>>(rents);
                    return Ok(rentsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAllRents() action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rent/{rate_id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}", Name = "RentById")]
        public async Task<IActionResult> GetRentById(int id)
        {
            try
            {
                bool userOwnsRent = await _repositoryWrapper.Rent.UserOwnsRent(HttpContext.SelectLoggedUserId(), id);
                if (!userOwnsRent)
                {
                    return Unauthorized();
                }
                var rent = await _repositoryWrapper.Rent.GetRentById(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rent with id: {id}");
                    var rentResult = _mapper.Map<RentDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentById(rent_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rent/tenant/{rate_id}
        [Authorize(Policy = "Landlord")]
        [HttpGet("tenant/{id}", Name = "RentsByTenantId")]
        public async Task<IActionResult> GetRentsByTenantId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsByTenantId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with TenantId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with tenant_id: {id}, Rents by tenant_id count: ${rents.Count()}");
                    var rentsResult = _mapper.Map<IEnumerable<RentDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside RentByTenantId(tenant_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rent/propert/{property_id}
        [Authorize(Policy = "Landlord")]
        [HttpGet("property/{id}", Name = "RentsByPropertyId")]
        public async Task<IActionResult> GetRentsByPropertyId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsByPropertyId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with PropertyId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with property_id: {id}, Rents by property_id count: ${rents.Count()}");
                    var rentsResult = _mapper.Map<IEnumerable<RentDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentsByPropertyId(property_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rent/tenant/{tenant_id}/start/{start_date}/end/{end_date}
        [Authorize(Policy = "Landlord")]
        [HttpGet("tenant/{id}/start/{start}/end/{end}", Name = "RentsByTenantIdBetweenDates")]
        public async Task<IActionResult> GetRentsByTenantIdBetweenDates(int id, DateTime start, DateTime end)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsByTenantIdBetweenDates(id, start, end);

                if (rents == null)
                {
                    _logger.LogError($"Rent with TenantId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with tenant_id: {id}, Rents between dates count: ${rents.Count()}");
                    var rentsResult = _mapper.Map<IEnumerable<RentDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentsByTenantIdBetweenDates(tenant_id, startRentDate, endRentDate) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get ongoing Rents by: {tenant_id}.
        /// Web version - unlimited.
        /// </summary>
        /// <returns>JSON: Rent[]</returns>
        // Web version (unlimited)
        //GET api/rent/tenant/{tenant_id}/ongoing
        [Authorize(Policy = "Landlord")]
        [HttpGet("tenant/{id}/ongoing", Name = "OngoingRentsByTenantId")]
        public async Task<IActionResult> GetRentsThatAreOngoingByTenantId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsThatHaveEndDateBeforeActualDateByTenantId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with TenantId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with tenant_id: {id}, ongoing Rents count: ${rents.Count()}");
                    var rentsResult = _mapper.Map<IEnumerable<RentDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentsThatAreOngoingByTenantId(tenant_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }
        /// <summary>
        /// Get ongoing Rents with related Property by: {tenant_id}.
        /// Web version - unlimited.
        /// </summary>
        /// <returns>JSON: Rent[] Including Property</returns>
        //GET api/rent/tenant/{tenant_id}/property/ongoing/
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("tenant/{id}/property/ongoing", Name = "OngoingRentsWithPropertyByTenantId")]
        public async Task<IActionResult> GetRentsThatAreOngoingWithPropertyByTenantId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsThatHaveEndDateBeforeActualDateWithPropertyByTenantId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with TenantId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with related Property by tenant_id: {id}, ongoing Rents count: {rents.Count()}");
                    var rentsResult = _mapper.Map<IEnumerable<RentWithPropertyDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentsThatAreOngoingWithPropertyByTenantId(tenant_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get ongoing Rents with related Property and related Property Address by: {tenant_id}.
        /// Web version - unlimited.
        /// </summary>
        /// <returns>JSON: Rent[] Including Property and Address</returns>
        //GET api/rent/tenant/{tenant_id}/property/address/ongoing/
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("tenant/{id}/property/address/ongoing", Name = "OngoingRentsWithPropertyAndAddressByTenantId")]
        public async Task<IActionResult> GetRentsThatAreOngoingWithPropertyAndAddressByTenantId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsThatHaveEndDateBeforeActualDateWithPropertyAndAddressByTenantId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with TenantId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with related Property and Address by tenant_id");
                    var rentsResult = _mapper.Map<IEnumerable<RentWithPropertyAndAddressDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentsThatAreOngoingWithPropertyAndAddressByTenantId(tenant_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        // AKCJA 1 LANDLORD

        /// <summary>
        /// Get all Rents with related Tenant and Property with related p.Address by: {landlord_id}.
        /// Web version - unlimited model.
        /// </summary>
        /// <returns>JSON: Rent[] Including Tenant, Property with Address</returns>
        //GET api/rent/landlord/{landlord_id}/tenant/property/address
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/tenant/property/address", Name = "RentsWithTenantAndPropertyWithAddressByLandlordId")]
        public async Task<IActionResult> GetRentsWithTenantAndPropertyWithAddressByLandlordId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsWithTenantAndPropertyWithAddressByLandlordId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with LandlordId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with related Property and Address by landlord_id.");
                    var rentsResult = _mapper.Map<IEnumerable<RentWithTenantAndPropertyWithAddressDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentsWithTenantAndPropertyWithAddressByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        // AKCJA 2 LANDLORD

        /// <summary>
        /// Get all Rents (no contract) with related Tenant and Property (no thumbnail) with related p.Address by: {landlord_id}.
        /// Web version - limited model.
        /// </summary>
        /// <returns>JSON: Rent[] Including Tenant, Property with Address</returns>
        //GET api/rent/landlord/{landlord_id}/tenant/property/address/ongoing/
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/tenant/property/address/limited", Name = "LimitedRentsWithTenantAndPropertyWithAddressByLandlordId")]
        public async Task<IActionResult> GetLimitedRentsWithTenantAndPropertyWithAddressByLandlordId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsWithTenantAndPropertyWithAddressByLandlordId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with LandlordId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with related Property and Address by landlord_id.");
                    var rentsResult = _mapper.Map<IEnumerable<RentWithTenantAndPropertyWithAddressLimitedDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLimitedRentsWithTenantAndPropertyWithAddressByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }


        // AKCJA 3 LANDLORD

        /// <summary>
        /// Get ongoing Rents with related Tenant, Property with p.Address by: {landlord_id}.
        /// Web version - unlimited model.
        /// </summary>
        /// <returns>JSON: Rent[] Including Tenant, Property with Address</returns>
        //GET api/rent/landlord/{landlord_id}/tenant/property/address/ongoing/
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/tenant/property/address/ongoing", Name = "OngoingRentsWithTenantAndPropertyWithAddressByLandlordId")]
        public async Task<IActionResult> GetRentsThatAreOngoingWithTenantAndPropertyWithAddressByLandlordId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsThatHaveEndDateBeforeActualDateWithTenantAndPropertyWithAddressByLandlordId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with LandlordId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with related Property and Address by landlord_id.");
                    var rentsResult = _mapper.Map<IEnumerable<RentWithTenantAndPropertyWithAddressDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentsThatAreOngoingWithTenantAndPropertyWithAddressByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }


        // AKCJA 4 LANDLORD

        /// <summary>
        /// Get ongoing Rents (no contract) with related Tenant and Property (no thumbnail) with related p.Address by: {landlord_id}.
        /// Web version - limited model.
        /// </summary>
        /// <returns>JSON: Rent[] Including Tenant, Property with Address</returns>
        //GET api/rent/landlord/{landlord_id}/tenant/property/address/ongoing/limited
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/tenant/property/address/ongoing/limited", Name = "OngoingLimitedRentsWithTenantAndPropertyWithAddressByLandlordId")]
        public async Task<IActionResult> GetLimitedRentsThatAreOngoingWithPropertyAndAddressByLandlordId(int id)
        {
            try
            {
                if(HttpContext.SelectLoggedUserId() != id)
                {
                    return Unauthorized();
                }
                var rents = await _repositoryWrapper.Rent.GetRentsThatHaveEndDateBeforeActualDateWithTenantAndPropertyWithAddressByLandlordId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with LandlordId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with related Property and Address by landlord_id.");
                    var rentsResult = _mapper.Map<IEnumerable<RentWithTenantAndPropertyWithAddressLimitedDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLimitedRentsThatAreOngoingWithPropertyAndAddressByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get finished Rents (no contract) with related Tenant and Property (no thumbnail) with related p.Address by: {landlord_id}.
        /// Web version - limited model.
        /// </summary>
        /// <returns>JSON: Rent[] Including Tenant, Property with Address</returns>
        //GET api/rent/landlord/{landlord_id}/tenant/property/address/finished/limited
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/tenant/property/address/finished/limited", Name = "FinishedLimitedRentsWithTenantAndPropertyWithAddressByLandlordId")]
        public async Task<IActionResult> GetLimitedRentsThatAreFinishedWithPropertyAndAddressByLandlordId(int id)
        {
            try
            {
                if (HttpContext.SelectLoggedUserId() != id)
                {
                    return Unauthorized();
                }
                var rents = await _repositoryWrapper.Rent.GetFinishedRentsWithTenantAndPropertyWithAddressByLandlordId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with LandlordId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with related Property and Address by landlord_id.");
                    var rentsResult = _mapper.Map<IEnumerable<RentWithTenantAndPropertyWithAddressLimitedDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLimitedRentsThatAreFinishedWithPropertyAndAddressByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get Rents (no contract) with related Tenant and Property (no thumbnail) with related p.Address by: {tenant_id}.
        /// Web version - limited model.
        /// </summary>
        /// <returns>JSON: Rent[] Including Tenant, Property with Address</returns>
        //GET api/rent/tenant/{id}/tenant/property/address/ongoing/limited
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("tenant/{id}/tenant/property/address/ongoing/limited", Name = "LimitedRentsWithTenantAndPropertyAndAddressByLandlordId")]
        public async Task<IActionResult> GetLimitedRentsWithTenantAndPropertyAndAddressByLandlordId(int id)
        {
            try
            {
                int landlordId = await _repositoryWrapper.Rent.FindLandlordIdByTenantId(id);
                if(HttpContext.SelectLoggedUserId() != landlordId)
                {
                    return Unauthorized();
                }
                var rents = await _repositoryWrapper.Rent.GetRentsWithTenantAndPropertyWithAddressByTenantId(id);
                
                if (rents == null)
                {
                    _logger.LogError($"Rent with LandlordId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with related Tenant, Property and Address by tenant_id.");
                    var rentsResult = _mapper.Map<IEnumerable<RentWithTenantAndPropertyWithAddressLimitedDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLimitedRentsWithTenantAndPropertyAndAddressByLandlordId(tenant_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }


        /// <summary>
        /// Get ongoing Rents by: {tenant_id}.
        /// App version - limited.
        /// </summary>
        /// <returns>JSON: Rent[]</returns>
        //GET api/rent/tenant/{tenant_id}/ongoing/limited
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("tenant/{id}/ongoing/limited", Name = "LimitedOngoingRentsByTenantId")]
        public async Task<IActionResult> GetLimitedRentsThatAreOngoingByTenantId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsThatHaveEndDateBeforeActualDateByTenantId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with TenantId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with tenant_id: {id}, ongoing Rents count: ${rents.Count()}");
                    var rentsResult = _mapper.Map<IEnumerable<RentForAppDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLimitedRentsThatAreOngoingByTenantId(tenant_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get ongoing Rents with related Property by: {tenant_id}.
        /// App version - limited.
        /// </summary>
        /// <returns>JSON: Rent[] Including Property</returns>
        //GET api/rent/tenant/{tenant_id}/property/ongoing/limited
        [Authorize(Policy = "Landlord")]
        [HttpGet("tenant/{id}/property/ongoing/limited", Name = "LimitedOngoingRentsWithPropertyByTenantId")]
        public async Task<IActionResult> GetLimitedRentsThatAreOngoingWithPropertyByTenantId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsThatHaveEndDateBeforeActualDateWithPropertyByTenantId(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with TenantId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with related Property by tenant_id: {id}, ongoing Rents count: {rents.Count()}");
                    var rentsResult = _mapper.Map<IEnumerable<RentWithPropertyForAppDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLimitedRentsThatAreOngoingWithPropertyByTenantId(tenant_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rent/tenant/{tenant_id}/finished
        [Authorize(Policy = "Landlord")]
        [HttpGet("tenant/{id}/finished", Name = "FinishedRentsByTenantId")]
        public async Task<IActionResult> GetRentsByTenantIdThatAreFinished(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.Rent.GetRentsByTenantIdThatAreFinished(id);

                if (rents == null)
                {
                    _logger.LogError($"Rent with TenantId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rents with tenant_id: {id}, finished Rents count: {rents.Count()}");
                    var rentsResult = _mapper.Map<IEnumerable<RentDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentsByTenantIdThatAreFinished(tenant_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rent/{id}/property
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/property", Name = "RentWithProperty")]
        public async Task<IActionResult> GetRentWithPropertyByRentId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.Rent.GetRentWithProperty(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with Id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rent with id: {id} with related property.");
                    var rentResult = _mapper.Map<RentWithPropertyDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentWithPropertyByRentId(rent_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get rent limited with property by {rent_id}
        /// (No contract included for fast transfer)
        /// </summary>
        /// <param name="id">rent_id</param>
        /// <returns>rent with property without contract</returns>
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/property/limited", Name = "LimitedRentWithProperty")]
        public async Task<IActionResult> GetLimitedRentWithPropertyByRentId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.Rent.GetRentWithProperty(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with Id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned limited Rent with id: {id} with related property.");
                    var rentResult = _mapper.Map<RentWithPropertyLimitedDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLimitedRentWithPropertyByRentId(rent_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get rent limited with property and address by {rent_id}
        /// (No contract included for fast transfer)
        /// </summary>
        /// <param name="id">rent_id</param>
        /// <returns>rent with property without contract</returns>
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/property/address/limited", Name = "LimitedRentWithPropertyAndAddress")]
        public async Task<IActionResult> GetLimitedRentWithPropertyAndAddressByRentId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.Rent.GetRentWithPropertyAndAddress(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with Id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned limited Rent with id: {id} with related property and address.");
                    var rentResult = _mapper.Map<RentWithPropertyAndAddressLimitedDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLimitedRentWithPropertyAndAddressByRentId(rent_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get rent limited with property and address and with rate by {rent_id}
        /// (No contract included for fast transfer)
        /// </summary>
        /// <param name="id">rent_id</param>
        /// <returns>rent with property without contract</returns>
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/property/address/rate/limited", Name = "LimitedRentWithPropertyAndAddressAndRate")]
        public async Task<IActionResult> GetLimitedRentWithPropertyAndAddressAndRateByRentId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.Rent.GetRentWithPropertyAndAddressAndRate(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with Id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned limited Rent with id: {id} with related property and address.");
                    var rentResult = _mapper.Map<RentWithPropertyAndAddressAndRateLimitedDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLimitedRentWithPropertyAndAddressAndRateByRentId(rent_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// This action will return rent by rent_id with related property + its address and tenant + its address
        /// </summary>
        /// <param name="id">rent_id</param>
        /// <returns> Rent,Tenant,Property (Addresses)</returns>
        //GET api/rent/{id}/property
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/property/tenant/address", Name = "RentWithPropertyTenantAndAddress")]
        public async Task<IActionResult> GetRentWithRelatedPropertyAndTenantAndTheirAddressByRentId(int id)
        {
            try
            {
                bool userOwnsRent = await _repositoryWrapper.Rent.UserOwnsRent(HttpContext.SelectLoggedUserId(), id);
                if (!userOwnsRent)
                {
                    return Unauthorized();
                }
                var rent = await _repositoryWrapper.Rent.GetRentWithPropertyTenantAndAddress(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with Id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rent with id: {id} with related property, tenant and addresses.");
                    var rentResult = _mapper.Map<RentWithPropertyTenantAndAddressDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentWithRelatedPropertyAndTenantAndTheirAddressByRentId(rent_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rent/{id}/state
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/state", Name = "RentWithStates")]
        public async Task<IActionResult> GetRentWithStatesByRentId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.Rent.GetRentWithStates(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rent with id: {id} and related states, states count: {rent.States.Count()}.");
                    var rentResult = _mapper.Map<RentWithStatesDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentWithStatesByRentId(rent_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rent/{id}/invoice
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/invoice")]
        public async Task<IActionResult> GetRentWithInvoicesByRentId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.Rent.GetRentWithInvoices(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rent with id: {id} and related invoices, invoices count: ${rent.Invoices.Count()}.");
                    var rentResult = _mapper.Map<RentWithInvoicesDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentWithInvoicesByRentId(rent_Id) action: {e.Message}");
                return StatusCode(500, e.ToString());
            }
        }

        //GET api/rent/{id}/landlord
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/landlord")]
        public async Task<IActionResult> GetRentWithLandlordByRentId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.Rent.GetRentWithLandlord(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rent with id: {id} with related landlord.");
                    var rentResult = _mapper.Map<RentWithLandlordDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentWithLandlordByRentId(rent_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rent/{id}/tenant
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/tenant")]
        public async Task<IActionResult> GetRentWithTenantByRentId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.Rent.GetRentWithTenant(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rent with id: {id} with related tenant.");
                    var rentResult = _mapper.Map<RentWithTenantDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentWithTenantByRentId(rent_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/rent/{id}/invoice/state
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/invoice/state")]
        public async Task<IActionResult> GetRentWithInvoiceAndStateByRentId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.Rent.GetRentWithInvoiceAndStates(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Rent with id: {id} with related invoices and states.");
                    var rentResult = _mapper.Map<RentWithInvoiceAndStateDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRentWithInvoiceAndStateByRentId(rent_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //POST api/rent
        [Authorize(Policy = "Landlord")]
        [HttpPost]
        public async Task<IActionResult> CreateRent([FromBody] RentForCreationDto rent)
        {
            try
            {

                if (rent == null)
                {
                    _logger.LogError("Rent received is a Null Object.");
                    return BadRequest("Rent object is null. Please send full request.");
                }
                else if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Rent object sent from client.");
                    return BadRequest("Rent object is not Valid");
                }
                else if (rent.StartRent > rent.EndRent)
                {
                    _logger.LogError("Invalid Rent object sent from client. StartRent date is <= EndRent date.");
                    return BadRequest("StartRent must be before EndRent.");
                }

                Boolean propertyExist = await _repositoryWrapper.Property.CheckIfPropertyExistByPropertyId(rent.PropertyId);
                Boolean tenantExist = await _repositoryWrapper.Tenant.CheckIfTenantExistByTenantId(rent.TenantId);
                Boolean landlordExist = await _repositoryWrapper.Landlord.CheckIfLandlordExistByLandlordId(rent.LandlordId);

                if(!propertyExist){
                    _logger.LogError("User tried to create new Rent with non existing PropertyId");
                    return BadRequest($"Not created. Property with id: {rent.PropertyId} does not exist in DB.");
                }
                else if (!tenantExist)
                {
                    _logger.LogError("User tried to create new Rent with non existing TenantId");
                    return BadRequest($"Not created. Tenant with id: {rent.TenantId} does not exist in DB.");
                }
                else if (!landlordExist)
                {
                    _logger.LogError("User tried to create new Rent with non existing LanlordId");
                    return BadRequest($"Not created. Landlord with id: {rent.LandlordId} does not exist in DB.");
                }

                var rentAlreadyInDB = await _repositoryWrapper.Rent.CheckIfSuchRentAlreadyExist(rent);
                if (rentAlreadyInDB != null)
                {
                    _logger.LogError("Rent with given PropertyId already exist in Db, cannot create.");
                    return BadRequest("Rent already in Database, cannot insert same rent twice.");
                }

                var rentEntity = _mapper.Map<Rent>(rent);

                _repositoryWrapper.Rent.CreateRent(rentEntity);
                await _repositoryWrapper.Save();

                var createdRent = _mapper.Map<RentDto>(rentEntity);
                return CreatedAtRoute("RentById", new { id = createdRent.Id }, createdRent);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside CreateRent(RentForCreationDto) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //PUT api/rent
        [Authorize(Policy = "Landlord")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRate(int id, [FromBody]RentForUpdateDto rent)
        {
            try
            {
                if (HttpContext.SelectLoggedUserId() != rent.LandlordId)
                {
                    return Unauthorized();
                }

                if (rent == null)
                {
                    _logger.LogError("Rent received is a Null Object.");
                    return BadRequest("Rent object is null. Please send full request.");
                }
                else if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Rent object sent from client.");
                    return BadRequest("Rent object is not Valid");
                }
                else if (rent.StartRent > rent.EndRent)
                {
                    _logger.LogError("Invalid Rent object sent from client. StartRent date is <= EndRent date.");
                    return BadRequest("StartRent must be before EndRent.");
                }

                Boolean renExist = await _repositoryWrapper.Rent.CheckIfRentExistByRentId(id);
                Boolean propertyExist = await _repositoryWrapper.Property.CheckIfPropertyExistByPropertyId(rent.PropertyId);
                Boolean tenantExist = await _repositoryWrapper.Tenant.CheckIfTenantExistByTenantId(rent.TenantId);
                Boolean landlordExist = await _repositoryWrapper.Landlord.CheckIfLandlordExistByLandlordId(rent.LandlordId);
                
                if (!renExist)
                {
                    _logger.LogError($"User tried to update non existing Rent with rent_id: {id}");
                    return BadRequest($"Not updated. Rent with id: {id} does not exist in DB.");
                }
                else if (!propertyExist)
                {
                    _logger.LogError("User tried to update Rent with non existing PropertyId");
                    return BadRequest($"Not updated. Property with id: {rent.PropertyId} does not exist in DB.");
                }
                else if (!tenantExist)
                {
                    _logger.LogError("User tried to update Rent with non existing TenantId");
                    return BadRequest($"Not updated. Tenant with id: {rent.TenantId} does not exist in DB.");
                }
                else if (!landlordExist)
                {
                    _logger.LogError("User tried to update Rent with non existing LanlordId");
                    return BadRequest($"Not updated. Landlord with id: {rent.LandlordId} does not exist in DB.");
                }
                

                var rentEntityFound = await _repositoryWrapper.Rent.GetRentById(id);

                if (rentEntityFound == null)
                {
                    _logger.LogError($"Rent with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(rent, rentEntityFound);
                _repositoryWrapper.Rent.UpdateRent(rentEntityFound);

                await _repositoryWrapper.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside UpdateRent(id, RentForUpdateDto) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //DELETE api/rent
        [Authorize(Policy = "Landlord")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRent(int id)
        {
            try
            {
                // Get Rent by Id with Invoices[] & States[] to check if they are constrained before delete action.
                var rent = await _repositoryWrapper.Rent.GetRentWithInvoiceAndStates(id);

                if (rent == null)
                {
                    _logger.LogError($"Not deleted. Rent with given id: {id} was not found");
                    return NotFound();
                }
                else if (rent.DisplayOnWeb == false)
                {
                    _logger.LogError($"Not deleted. Rent with given id: {id} is already displayOnWeb == false");
                    return BadRequest("Rent is already deleted");
                }
                else if (rent.Invoices.Count() > 0)
                {
                    rent.DisplayOnWeb = false;
                    var rentEntity = _mapper.Map<Rent>(rent);
                    _repositoryWrapper.Rent.UpdateRent(rentEntity);
                    await _repositoryWrapper.Save();
                    return Ok(); // "Rent has related Invoices, that were generated for existing States and shall not be removed beacause it has real documents. Rent will NOT be displayed on Web but still exist in DB"
                }
                else if (rent.States.Count() == 1)
                {
                    State ShoudBeInitialState = rent.States.AsEnumerable().ElementAt(0);
                    
                    if (ShoudBeInitialState.IsInitial)
                        _repositoryWrapper.State.DeleteState(ShoudBeInitialState);
                    else
                    {
                        rent.DisplayOnWeb = false;
                        var rentEntity = _mapper.Map<Rent>(rent);
                        _repositoryWrapper.Rent.UpdateRent(rentEntity);
                        await _repositoryWrapper.Save();
                        return Ok(); //("Rent has related States that were set by Tenant and shall not be removed, beacuse states are not Initial. Rent will not be displayed on Web, but still exist in DB")
                    }
                }
                else if (rent.States.Count() > 1)
                {
                    uint InitialStatesCount = 0;

                    foreach (State state in rent.States)
                    {
                        InitialStatesCount = (state.IsInitial == true) ? InitialStatesCount += 1 : InitialStatesCount += 0;
                    }

                    if (InitialStatesCount == rent.States.Count())
                        foreach (State state in rent.States)
                        {
                            _repositoryWrapper.State.DeleteState(state);
                        }
                    else
                    {
                        rent.DisplayOnWeb = false;
                        var rentEntity = _mapper.Map<Rent>(rent);
                        _repositoryWrapper.Rent.UpdateRent(rentEntity);
                        await _repositoryWrapper.Save();
                        return Ok(); // "Rent has related States that were set by Tenant and shall not be removed, beacuse states are are not Initial. Rent will not be displayed on Web, but still exist in DB"
                    }

                }

                _repositoryWrapper.Rent.DeleteRent(rent);
                await _repositoryWrapper.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside DeleteRent(rent_id) action: {e.Message}");
                return StatusCode(500, e.ToString());
            }
        }





    }
}
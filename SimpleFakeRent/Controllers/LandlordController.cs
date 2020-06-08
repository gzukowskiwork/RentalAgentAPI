using System;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using SimpleFakeRent.Extensions;
using System.Linq;
using Contracts;
using System.Collections.Generic;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Landlord;
using Microsoft.AspNetCore.Authorization;
using Identity.Services;
using EmailService;

namespace SimpleFakeRent.Controllers
{
    [Route("api/landlord")]
    [ApiController]
    public class LandlordController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IIdentityService _identityService;
        private readonly IEmailEmmiter _emailEmmiter;

        public LandlordController(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger, IIdentityService identityService, IEmailEmmiter emailEmmiter)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _identityService = identityService;
            _emailEmmiter = emailEmmiter;
        }

        //GET api/landlord
        [HttpGet]
        public async Task<IActionResult> GetAllLandlords()
        {
            try
            {
                var landlords = await _repositoryWrapper.Landlord.GetAllLandlords();

                if (landlords == null)
                {
                    _logger.LogInfo($"No 'Landlord' has been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned all 'Landlords' from db. Number of Landords found: {landlords.Count()}.");
                    var landlordsResult = _mapper.Map<IEnumerable<LandlordDto>>(landlords);
                    return Ok(landlordsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAllLandlords() action: {e.Message}");
                _logger.LogError(e.ErrorMessageExtension());
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "Landlord,Tenant")]
        //GET api/landlord/id
        [HttpGet("{id}", Name = "LandlordById")]
        public async Task<IActionResult> GetLandlordById(int id)
        {
            try
            {
                var landlord = await _repositoryWrapper.Landlord.GetLandlordById(id);

                if (landlord == null)
                {
                    _logger.LogError($"Landlord with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with id: {id}");
                    var landlordResult = _mapper.Map<LandlordDto>(landlord);
                    return Ok(landlordResult);
                }
                    
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordById(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/{id}/property
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/property")]
        public async Task<IActionResult> GetLandlordWithProperties(int id)
        {
            try
            {
                var landlord = await _repositoryWrapper.Landlord.GetLandlordWithProperties(id);

                if (landlord == null)
                {
                    _logger.LogError($"Landlord with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with id: {id} and its properties. Number of Properties of this landlord: {landlord.Properties.Count()}.");
                    var landlordResult = _mapper.Map<LandlordWithPropertiesDto>(landlord);
                    return Ok(landlordResult);
                }
                    
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordWithProperties(landlord_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/{id}/address
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/address")]
        public async Task<IActionResult> GetLandlordWithAddress(int id)
        {
            try
            {
                bool isValidUser = await _repositoryWrapper.Landlord.CheckIfUserOwnsAddress(HttpContext.SelectLoggedUserId(), id);
                if (!isValidUser)
                {
                    return Unauthorized();
                }
                var landlord = await _repositoryWrapper.Landlord.GetLandlordWithAddress(id);

                if (landlord == null)
                {
                    _logger.LogError($"Landlord with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord and its address.");
                    var landlordResult = _mapper.Map<LandlordWithAddressDto>(landlord);
                    return Ok(landlordResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordWithAddress(landlord_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/city/gdansk
        [Authorize(Policy = "Landlord")]
        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetLandlordsByCity(string city)
        {
            try
            {
                var landlords = await _repositoryWrapper.Landlord.GetLandlordsByCity(city);

                if (landlords == null)
                {
                    _logger.LogError($"Landlords with city: {city}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with city: {city}, landlords found: {landlords.Count()}.");
                    var landlordsResult = _mapper.Map<IEnumerable<LandlordDto>>(landlords);
                    return Ok(landlordsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordsByCity(city) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/street/pionkowa
        [Authorize(Policy = "Landlord")]
        [HttpGet("street/{street}")]
        public async Task<IActionResult> GetLandlordsByStreet(string street)
        {
            try
            {
                var landlords = await _repositoryWrapper.Landlord.GetLandlordsByStreet(street);

                if (landlords == null)
                {
                    _logger.LogError($"Landlord with Street: {street}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with Street: {street}, landlords found: {landlords.Count()}.");
                    var landlordsResult = _mapper.Map<IEnumerable<LandlordDto>>(landlords);
                    return Ok(landlordsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordsByStreet(street) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/country/polska
        [Authorize(Policy = "Landlord")]
        [HttpGet("country/{country}")]
        public async Task<IActionResult> GetLandlordsByCountry(string country)
        {
            try
            {
                var landlords = await _repositoryWrapper.Landlord.GetLandlordsByCountry(country);

                if (landlords == null)
                {
                    _logger.LogError($"Landlord with Country: {country}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with Country: {country}, landlords found: {landlords.Count()}.");
                    var landlordsResult = _mapper.Map<IEnumerable<LandlordDto>>(landlords);
                    return Ok(landlordsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordsByCountry(country) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/zip/80-419
        [Authorize(Policy = "Landlord")]
        [HttpGet("zip/{zipCode}")]
        public async Task<IActionResult> GetLandlordsByZipCode(string zipCode)
        {
            try
            {
                var landlords = await _repositoryWrapper.Landlord.GetLandlordsByZipCode(zipCode);

                if (landlords == null)
                {
                    _logger.LogError($"Landlord with ZipCode: {zipCode}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with ZipCode: {zipCode}, landlords found: {landlords.Count()}.");
                    var landlordsResult = _mapper.Map<IEnumerable<LandlordDto>>(landlords);
                    return Ok(landlordsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordsByZipCode(zipCode) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/email/d.nowak@gmail.com
        [Authorize(Policy = "Landlord")]
        [HttpGet("email/{email}", Name = "OwnerByEmail")]
        public async Task<IActionResult> GetLandlordByEmail(string email)
        {
            try
            {
                var landlord = await _repositoryWrapper.Landlord.GetLandlordByEmail(email);

                if (landlord == null)
                {
                    _logger.LogError($"Landlord with e-mail: {email}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with e-mail: {email}.");
                    var landlordResult = _mapper.Map<LandlordDto>(landlord);
                    return Ok(landlordResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordByEmail(email) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/asp/{user_id}
        [Authorize(Policy = "Landlord")]
        [HttpGet("asp/{id}", Name = "LandlordByAspNetUserId")]
        public async Task<IActionResult> GetLandlordByAspUserId(int id)
        {
            try
            {
                var landlord = await _repositoryWrapper.Landlord.GetLandlordByAspNetUserId(id);

                if (landlord == null)
                {
                    _logger.LogError($"Landlord with aspId: {id}, hasn't been found in db.");
                    return NotFound("Tenant not found.");
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with aspId: {id}.");
                    var landlordResult = _mapper.Map<LandlordDto>(landlord);
                    return Ok(landlordResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordByAspUserId(aspNetUserId) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/surname/nowak
        [Authorize(Policy = "Landlord")]
        [HttpGet("surname/{surname}")]
        public async Task<IActionResult> GetLandlordsBySurname(string surname)
        {
            try
            {
                var landlords = await _repositoryWrapper.Landlord.GetLandlordsBySurname(surname);

                if (landlords == null)
                {
                    _logger.LogError($"Landlord with Surname: {surname}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with Surname: {surname}, landlords found: {landlords.Count()}.");
                    var landlordsResult = _mapper.Map<IEnumerable<LandlordDto>>(landlords);
                    return Ok(landlordsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordsBySurname(landlord_surname) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/company/Lubie Kulki
        [Authorize(Policy = "Landlord")]
        [HttpGet("company/{company}")]
        public async Task<IActionResult> GetLandlordsByCompany(string company)
        {
            try
            {
                var landlords = await _repositoryWrapper.Landlord.GetLandlordByCompanyName(company);

                if (landlords == null)
                {
                    _logger.LogError($"Landlord with CompanyName: {company}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with CompanyName: {company}, landlords found: {landlords.Count()}.");
                    var landlordsResult = _mapper.Map<IEnumerable<LandlordDto>>(landlords);
                    return Ok(landlordsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordsByCompany(company_name) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/nip/1831961319 -> 10 digit
        [Authorize(Policy = "Landlord")]
        [HttpGet("nip/{nip}")]
        public async Task<IActionResult> GetLandlordsByNip(string nip)
        {
            try
            {
                var landlords = await _repositoryWrapper.Landlord.GetLandlordsByNip(nip);

                if (landlords == null)
                {
                    _logger.LogError($"Landlord with NIP: {nip}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with NIP: {nip}, landlords found: {landlords.Count()}.");
                    var landlordsResult = _mapper.Map<IEnumerable<LandlordDto>>(landlords);
                    return Ok(landlordsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordByNip(NIP) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/pesel/89092977755 -> 11 digit // Returns a list (to check on e.g. on delete action before development version)
        [Authorize(Policy = "Landlord")]
        [HttpGet("pesel/{pesel}")]
        public async Task<IActionResult> GetLandlordsByPesel(string pesel)
        {
            try
            {
                var landlords = await _repositoryWrapper.Landlord.GetLandlordsByPesel(pesel);

                if (landlords == null)
                {
                    _logger.LogError($"Landlord with PESEL: {pesel}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with PESEL: {pesel}, landlords found: {landlords.Count()}.");
                    var landlordsResult = _mapper.Map<IEnumerable<LandlordDto>>(landlords);
                    return Ok(landlordsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordByPesel(PESEL) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/landlord/regon/12345678910
        [Authorize(Policy = "Landlord")]
        [HttpGet("regon/{regon}")]
        public async Task<IActionResult> GetLandlordsByRegon(string regon)
        {
            try
            {
                var landlords = await _repositoryWrapper.Landlord.GetLandlordByRegon(regon);

                if (landlords == null)
                {
                    _logger.LogError($"Landlord with REGON: {regon}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Landlord with NIP: {regon}, landlords found: {landlords.Count()}.");
                    return Ok(landlords);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetLandlordsByRegon(regon) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }


        //POST api/landlord 
        [Authorize(Policy = "Landlord")]
        [HttpPost]
        public async Task<IActionResult> CreateLandlord([FromBody] LandlordForCreationDto landlord)
        {
            try
            {
                if (landlord == null)
                {
                    _logger.LogError("Landlord received is a Null Object.");
                    return BadRequest("Landlord object is null. Please send full request.");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Landlord object sent from client.");
                    return BadRequest("Landlord object is not Valid");
                }

                bool landlordExists = await _repositoryWrapper.Landlord.CheckIfLandlordWithAspNetUserIdExist(landlord.AspNetUsersId);
                bool tenantExists = await _repositoryWrapper.Tenant.CheckIfTenantWithAspNetUserIdExist(landlord.AspNetUsersId);

                if(landlordExists || tenantExists)
                {
                    return BadRequest($"Landlord with this asp_id already exists.");
                }

                bool aspNetUserExist = await _identityService.CheckIfAspNetUserExistByAspNetUserId(landlord.AspNetUsersId);

                if (!aspNetUserExist)
                {
                    return BadRequest($"Cannot create landlord. AspNetUser with given id does not exist in db.");
                }

                bool addressExist = await _repositoryWrapper.Address.CheckIfAddressExistByAddressId(landlord.AddressId);

                if (!addressExist)
                {
                    return BadRequest($"Cannot create landlord. Address with given id does not exist in db.");
                }

                // send confirmation email to landlord
                ApplicationUser aspNetUser = await _identityService.GetApplicationUserById(landlord.AspNetUsersId);
                Email email = new Email(aspNetUser.Email, "Registration in service", "\nSerdecznie witamy w naszym serwisie. " +
                    "\nA może dopiero co wynająłeś mieszkanie u kogoś, kto jest klientem RentalAgent i przygotowuje Twój profil do aplikacji? " +
                    "\nJeśli jednak to nie Ty właśnie się zarejestrowałeś używając tego maila, lub nic Ci na ten temat nie wiadomo, koniecznie zgłoś sprawę naszej administracji! " +
                    "\nPozdrawiamy!");
                await _emailEmmiter.SendUserRegistrationConfirmationEmail(email);

                var landlordEntity = _mapper.Map<Landlord>(landlord);
                
                _repositoryWrapper.Landlord.CreateLandlord(landlordEntity);
                await _repositoryWrapper.Save();

                var createdLandlord = _mapper.Map<LandlordDto>(landlordEntity);
                return CreatedAtRoute("LandlordByAspNetUserId", new { id = landlord.AspNetUsersId }, createdLandlord);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside CreateLandlord() action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //PUT api/landlord/{id}
        [Authorize(Policy = "Landlord")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLandlord(int id, [FromBody]LandlordForUpdateDto landlord)
        {
            try
            {
                if(HttpContext.SelectLoggedUserId() != id)
                {
                    return Unauthorized();
                }
                if (landlord == null)
                {
                    _logger.LogError("Landlord received is a Null Object.");
                    return BadRequest("Landlord object is null. Please send full request.");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Landlord object sent from client.");
                    return BadRequest("Landlord object is not Valid");
                }

                var landlordEntity = await _repositoryWrapper.Landlord.GetLandlordById(id);
                
                // Check if any person returned before updating
                if ( landlordEntity == null )
                {
                    _logger.LogError($"Landlord with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                                
                _mapper.Map(landlord, landlordEntity); 
                _repositoryWrapper.Landlord.UpdateLandlord(landlordEntity);

                await _repositoryWrapper.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside UpdateLandlord(id, landlordForUpdateDto) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //DELETE api/landlord
        [Authorize(Policy = "Landlord")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLandlord(int id)
        {
            try
            {
                var landlord = await _repositoryWrapper.Landlord.GetLandlordWithAddress(id);
                
                if (landlord == null)
                {
                    _logger.LogError($"Not deleted. Landlord with given id: {id} was not found");
                    return NotFound();
                }

                /*Boolean landlordHasProperties = await _repositoryWrapper.Property.CheckIfPropertyRelatedToLandlordExistByLandlordId(id);
                Boolean landlordHasRents = await _repositoryWrapper.Rent.CheckIfRentRelatedToLandlordExistByLandlordId(id);
                if (landlordHasProperties)
                {
                    _logger.LogError($"Not deleted. Landlord has Properties related. Delete them first");
                    return BadRequest("Landlord has properties. Delete them first.");
                }
                else if (landlordHasRents)
                {
                    _logger.LogError($"Not deleted. Landlord has Rents related. Delete them first");
                    return BadRequest("Landlord has rents. Delete them first.");
                }*/

                _repositoryWrapper.Landlord.DeleteLandlord(landlord);
                _repositoryWrapper.Address.DeleteAddress(landlord.Address);
                await _repositoryWrapper.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside DeleteLandlord(id) action: {e.ToString()}");
                return StatusCode(500, e.ToString());
            }
        }
    }
}
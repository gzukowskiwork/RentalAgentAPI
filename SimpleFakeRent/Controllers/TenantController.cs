using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Entities.DataTransferObjects.Tenant;
using SimpleFakeRent.Extensions;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Identity.Response;
using EmailService;
using Entities.DataTransferObjects.Address;

namespace SimpleFakeRent.Controllers
{
    [Route("api/tenant")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IIdentityService _identityService;
        private readonly IEmailEmmiter _emailEmmiter;

        public TenantController(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger, IIdentityService identityService, IEmailEmmiter emailEmmiter)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _identityService = identityService;
            _emailEmmiter = emailEmmiter;
        }

        //GET api/tenant
        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            try
            {
                var tenants = await _repositoryWrapper.Tenant.GetAllTenants();

                if (tenants == null)
                {
                    _logger.LogInfo($"No 'Tenant' has been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned all 'Tenants' from db. Number of Tenants found: {tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside .GetAllTenants() action: {e.Message}");
                _logger.LogError(e.ErrorMessageExtension());
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/9
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}", Name ="tenantById")]
        public async Task<IActionResult> GetTenantById(int id)
        {
            try
            {
                var tenant = await _repositoryWrapper.Tenant.GetTenantById(id);

                if (tenant == null)
                {
                    _logger.LogError($"Tenant with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenant with id: {id}");
                    var landlordResult = _mapper.Map<TenantDto>(tenant);
                    return Ok(landlordResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantById(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/9/address
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/address")]
        public async Task<IActionResult> GetTenantWithAddress(int id)
        {
            try
            {
                /////////////////////////////////////////////////
                int landlordId = await _repositoryWrapper.Rent.FindLandlordIdByTenantId(id);
                if (HttpContext.SelectLoggedUserId() != landlordId)
                {
                    return Unauthorized();
                }
                //////////////////////////////////////////////
                var tenant = await _repositoryWrapper.Tenant.GetTenantWithAddress(id);

                if (tenant == null)
                {
                    _logger.LogError($"Tenant with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenant with id: {id} and its address.");
                    var tenantResult = _mapper.Map<TenantWithAddressDto>(tenant);
                    return Ok(tenantResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantWithAddress(tenantId) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/email/kowalski@o2.pl
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("email/{email}", Name = "tenantByEmail")]
        public async Task<IActionResult>GetTenantByEmail(string email)
        {
            try
            {
                var tenant = await _repositoryWrapper.Tenant.GetTenantByEmail(email);

                if (tenant == null)
                {
                    _logger.LogError($"Tenant with e-mail: {email}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenant with e-mail: {email}.");
                    var tenantResult = _mapper.Map<TenantDto>(tenant);
                    return Ok(tenantResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantByEmail(email) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/asp/{user_id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("asp/{id}", Name = "TenantByAspNetUserId")]
        public async Task<IActionResult> GetTenantByAspUserId(int id)
        {
            try
            {
                var tenant = await _repositoryWrapper.Tenant.GetTenantByAspNetUserId(id);

                if (tenant == null)
                {
                    _logger.LogError($"Tenant with aspId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenant with aspId: {id}.");
                    var tenantResult = _mapper.Map<TenantDto>(tenant);
                    return Ok(tenantResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantByAspUserId(aspNetUserId) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/phone/123456798
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("phone/{phoneNumber}", Name = "tenantByPhoneNumber")]
        public async Task<IActionResult> GetTenantByPhoneNumber(string phoneNumber)
        {
            try
            {
                var tenant = await _repositoryWrapper.Tenant.GetTenantByPhone(phoneNumber);

                if (tenant == null)
                {
                    _logger.LogError($"Tenant with phone No.: {phoneNumber} (no prefix), hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenant with phone No.: {phoneNumber}.");
                    var tenantResult = _mapper.Map<TenantDto>(tenant);
                    return Ok(tenantResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantByPhoneNumber(phoneNumber) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/nip/1234567890
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("nip/{nip}", Name = "tenantByNip")]
        public async Task<IActionResult> GetTenantsByNip(string nip)
        {
            try
            {
                var tenants = await _repositoryWrapper.Tenant.GetTenantsByNip(nip);

                if (tenants == null)
                {
                    _logger.LogError($"Tenant with NIP: {nip}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenants with NIP: {nip}, tenant count :{tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantByNip(nip) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/pesel/89092977777
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("pesel/{pesel}", Name = "tenantByPesel")]
        public async Task<IActionResult> GetTenantsByPesel(string pesel)
        {
            try
            {
                var tenants = await _repositoryWrapper.Tenant.GetTenantsByPesel(pesel);

                if (tenants == null)
                {
                    _logger.LogError($"Tenant with PESEL: {pesel}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenants with PESEL: {pesel}, tenant count :{tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantsByPesel(pesel) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/company/true
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("company/{isCompany}", Name = "tenantByIsCompany")]
        public async Task<IActionResult> GetTenantsByIsCompany(bool isCompany)
        {
            try
            {
                var tenants = await _repositoryWrapper.Tenant.GetTenantsByIsCompany(isCompany);

                if (tenants == null)
                {
                    _logger.LogError($"Tenant with IsCompany: {isCompany}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenants with IsCompany: {isCompany}, tenant count :{tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantsByIsCompany(bool isCompany) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }
        //GET api/tenant/companyName/Argo
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("companyName/{company}")]
        public async Task<IActionResult> GetTenantsByCompany(string company)
        {
            try
            {
                var tenants = await _repositoryWrapper.Tenant.GetTenantsByCompanyName(company);

                if (tenants == null)
                {
                    _logger.LogError($"Tenants with CompanyName: {company}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenants with CompanyName: {company}, tenants found: {tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantsByCompany(company_name) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }
        //GET api/tenant/surname/kowalski
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("surname/{surname}")]
        public async Task<IActionResult> GetTenantsBySurname(string surname)
        {
            try
            {
                var tenants = await _repositoryWrapper.Tenant.GetTenantsBySurname(surname);

                if (tenants == null)
                {
                    _logger.LogError($"Tenant with Surname: {surname}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenants with Surname: {surname}, tenants found: {tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantsBySurname(tenant_surname) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/city/zakopane
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetTenantsByCity(string city)
        {
            try
            {
                var tenants = await _repositoryWrapper.Tenant.GetTenantsByCity(city);

                if (tenants == null)
                {
                    _logger.LogError($"Tenant with city: {city}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenants with city: {city}, tenants found: {tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantsByCity(city_name) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/street/gorska
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("street/{street}")]
        public async Task<IActionResult> GetTenantsByStreet(string street)
        {
            try
            {
                var tenants = await _repositoryWrapper.Tenant.GetTenantsByStreet(street);

                if (tenants == null)
                {
                    _logger.LogError($"Tenant with Street: {street}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenants with Street: {street}, tenants found: {tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantsByStreet(street) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/country/polska
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("country/{country}")]
        public async Task<IActionResult> GetTenantsByCountry(string country)
        {
            try
            {
                var tenants = await _repositoryWrapper.Tenant.GetTenantsByCountry(country);

                if (tenants == null)
                {
                    _logger.LogError($"Tenant with Country: {country}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenants with Country: {country}, tenants found: {tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantsByCountry(country) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/tenant/zip/80-256
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("zip/{zipCode}")]
        public async Task<IActionResult> GetTenantsByZipCode(string zipCode)
        {
            try
            {
                var tenants = await _repositoryWrapper.Tenant.GetTenantsByZipCode(zipCode);

                if (tenants == null)
                {
                    _logger.LogError($"Tenant with ZipCode: {zipCode}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Tenants with ZipCode: {zipCode}, tenants found: {tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantsByZipCode(zipCode) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }
        /// <summary>
        /// Get all tenants [] related with landlord (rented in past or currently rent a flat)
        /// Tenants are selected distinct by LandlordId (from Rent Entity).
        /// </summary>
        /// <param name="id"> LandlordId </param>
        /// <returns></returns>
        //GET api/tenant/landlord/{landlord_id}/all
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/all", Name = "relatedTenantsByLandlordId")]
        public async Task<IActionResult> GetRelatedTenantsByLandlordId(int id)
        {
            try
            {
                bool landlordExists = await _repositoryWrapper.Landlord.CheckIfLandlordExistByLandlordId(id);

                if(!landlordExists)
                {
                    return NotFound();
                }

                IEnumerable<Tenant> tenants = await _repositoryWrapper.Rent.GetAllTenantsByLandlordId(id);

                if (!tenants.Any())
                {
                    _logger.LogInfo($"No Tenants with LandordId: {id} has been found in db.");
                    return Ok(tenants);
                }
                else
                {
                    _logger.LogInfo($"Returned Tenants with LandlordId: {id}, tenant count :{tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRelatedTenantsByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get all tenants with extra field: e-mail by landlord_id
        /// </summary>
        /// <param name="id">landlordId</param>
        /// <returns>Tenant[]</returns>
        //GET api/tenant/landlord/{landlord_id}/all/email
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/all/email", Name = "relatedTenantsWithEmailByLandlordId")]
        public async Task<IActionResult> GetRelatedTenantsWithEmailByLandlordId(int id)
        {
            try
            {
                bool landlordExists = await _repositoryWrapper.Landlord.CheckIfLandlordExistByLandlordId(id);

                if (!landlordExists)
                {
                    return NotFound();
                }

                IEnumerable<Tenant> tenants = await _repositoryWrapper.Rent.GetAllTenantsWithApplicationUserByLandlordId(id);

                if (!tenants.Any())
                {
                    _logger.LogInfo($"No Tenants with LandordId: {id} has been found in db.");
                    return Ok(tenants);
                }
                else
                {
                    List<TenantWithEmailDto> tenantsWithEmail = new List<TenantWithEmailDto>();
                    foreach(Tenant tenant in tenants)
                    {
                        var tenantEmail = tenant.ApplicationUser.Email;
                        TenantWithEmailDto tenantWithEmail = _mapper.Map<TenantWithEmailDto>(tenant);
                        tenantWithEmail.Email = tenantEmail;
                        tenantsWithEmail.Add(tenantWithEmail);
                    }
                    _logger.LogInfo($"Returned Tenants by LandlordId: {id} with e-mail, tenant count :{tenants.Count()}.");
                    //var tenantsResult = _mapper.Map<IEnumerable<TenantWithEmailDto>>(tenants);
                    return Ok(tenantsWithEmail);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRelatedTenantsWithEmailByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get actual tenants [] related with landlord ( which rent is ongoing )
        /// Tenants are selected distinct by LandlordId (from Rent Entity) and EndRent: Date
        /// </summary>
        /// <param name="id"> LandlordId </param>
        /// <returns>200 - list of tenants included / 404- landlord not found </returns>
        //GET api/tenant/landlord/{landlord_id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/actual", Name = "actualTenantsByLandlordId")]
        public async Task<IActionResult> GetRelatedActualTenantsByLandlordId(int id)
        {
            try
            {
                //////////////////////////////////////////////////////
                if(HttpContext.SelectLoggedUserId() != id)
                {
                    return Unauthorized();
                }
                ////////////////////////////////////////////////////

                bool landlordExists = await _repositoryWrapper.Landlord.CheckIfLandlordExistByLandlordId(id);

                if (!landlordExists)
                {
                    return NotFound();
                }

                IEnumerable<Tenant> tenants = await _repositoryWrapper.Rent.GetActualTenantsByLandlordId(id);

                if (!tenants.Any())
                {
                    _logger.LogInfo($"No Tenants with LandordId: {id} has been found in db.");
                    return Ok(tenants);
                }
                else
                {
                    _logger.LogInfo($"Returned Tenants with LandlordId: {id}, tenant count :{tenants.Count()}.");
                    var tenantsResult = _mapper.Map<IEnumerable<TenantWithAddressDto>>(tenants);
                    return Ok(tenantsResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetRelatedActualTenantsByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }
        /// <summary>
        /// Returns existing tenant object from DB if such exist.
        /// 200 success, 400 if mismatch, 500 if unexpected error.
        /// </summary>
        /// <param name="tenant"> Tenant param. consist of Email, NIP, PESEL, Phone</param>
        /// <returns></returns>
        [Authorize(Policy = "Landlord")]
        [HttpPost("check")]
        public async Task<IActionResult> GetExistingTenantByTenantDetails([FromBody] TenantForCheckDto tenant)
        {
            try
            {
                Tenant tenantExists = await _repositoryWrapper.Tenant.GetTenantByTenantDetails(tenant);
                ApplicationUser applicationUser = await _identityService.GetApplicationUserTenantByEmail(tenant.Email);

                /* verify if tenant exist, and is tenant as AppUser */
                if(tenantExists == null || applicationUser == null )
                {
                    _logger.LogError($"Landlord tried to check if tenant exist, but tenant, or application user does not exist: tenant email: {tenant.Email}");
                    return BadRequest($"No such user or provided user details incorrect.");
                }
                /* application user constrainted with tenant */
                if(tenantExists.AspNetUsersId != applicationUser.Id)
                {
                    _logger.LogError($"Landlord tried to check if tenant exist, but provided application user id: {applicationUser.Id} mismatch provided tenant id: {tenantExists.AspNetUsersId}");
                    return BadRequest("Tenant and application user mismatch");
                }

                _logger.LogInfo($"Tenant in applicationUser: {applicationUser.Tenant} landlord: {applicationUser.Landlord}");
                return Ok(tenantExists);

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetTenantByTenantDetails(TenantForCheckDto) action: {e.ToString()}");
                return StatusCode(500, e.Message);
            }

        }

        //POST api/tenant
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] TenantForCreationDto tenant)
        {
            try
            {
                if (tenant == null)
                {
                    _logger.LogError("Tenant received is a Null Object.");
                    return BadRequest("Tenant object is null. Please send full request.");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Tenant object sent from client.");
                    return BadRequest("Tenant object is not Valid");
                }

                bool landlordExists = await _repositoryWrapper.Landlord.CheckIfLandlordWithAspNetUserIdExist(tenant.AspNetUsersId);
                bool tenantExists = await _repositoryWrapper.Tenant.CheckIfTenantWithAspNetUserIdExist(tenant.AspNetUsersId);

                if (landlordExists || tenantExists)
                {
                    return BadRequest($"Tenant with this asp_id already exists.");
                }

                bool aspNetUserExist = await _identityService.CheckIfAspNetUserExistByAspNetUserId(tenant.AspNetUsersId);

                if (!aspNetUserExist)
                {
                    return BadRequest($"Cannot create tenant. AspNetUser with given id does not exist in db.");
                }

                bool addressExist = await _repositoryWrapper.Address.CheckIfAddressExistByAddressId(tenant.AddressId);

                if (!addressExist)
                {
                    return BadRequest($"Cannot create tenant. Address with given id does not exist in db.");
                }

                // send confirmation email to landlord
                ApplicationUser aspNetUser = await _identityService.GetApplicationUserById(tenant.AspNetUsersId);
                Email email = new Email(aspNetUser.Email, "Registration in service", "\nSerdecznie witamy w naszym serwisie. " +
                    "\nA może dopiero co wynająłeś mieszkanie u kogoś, kto jest klientem RentalAgent i przygotowuje Twój profil do aplikacji? " +
                    "\nJeśli jednak to nie Ty właśnie się zarejestrowałeś używając tego maila, lub nic Ci na ten temat nie wiadomo, koniecznie zgłoś sprawę naszej administracji! " +
                    "\nPozdrawiamy!");
                await _emailEmmiter.SendUserRegistrationConfirmationEmail(email);

                var tenantEntity = _mapper.Map<Tenant>(tenant);

                _repositoryWrapper.Tenant.CreateTenant(tenantEntity);
                await _repositoryWrapper.Save();

                var createTenant = _mapper.Map<TenantWithAddressDto>(tenantEntity);
                var address = await _repositoryWrapper.Address.GetAddressById(createTenant.AddressId);
                createTenant.Address = _mapper.Map<AddressDto>(address);
                return CreatedAtRoute("TenantByAspNetUserId", new { id = createTenant.AspNetUsersId }, createTenant);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside CreateTenant(tenantForCreationDto) action: {e.ToString()}");
                return StatusCode(500, e.Message);
            }
        }

        //PUT api/tenant/{id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTenant(int id, [FromBody]TenantForUpdateDto tenant)
        {
            try
            {
                int landlordId = await _repositoryWrapper.Rent.FindLandlordIdByTenantId(id);
                if (HttpContext.SelectLoggedUserId() != landlordId)
                {
                    return Unauthorized();
                }
                if (tenant == null)
                {
                    _logger.LogError("Tenant received is a Null Object.");
                    return BadRequest("Tenant object is null. Please send full request.");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Tenant object sent from client.");
                    return BadRequest("Tenant object is not Valid");
                }

                // var tenantEntity = await _repositoryWrapper.Tenant.GetTenantsById(id);
                var tenantEntity = await _repositoryWrapper.Tenant.GetTenantWithAddress(id);

                // Check if any person returned before updating
                if (tenantEntity == null)
                {
                    _logger.LogError($"Tenant with id: {id}, hasn't been found in db.");
                    return NotFound("Tenant not found.");
                }

                _mapper.Map(tenant, tenantEntity);
                _repositoryWrapper.Tenant.UpdateTenant(tenantEntity);

                await _repositoryWrapper.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside UpdateTenant(id, tenantForUpdateDto) action: {e.ToString()}");
                return StatusCode(500, e.Message);
            }
        }

        //DELETE api/tenant/{tenant_id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenant(int id)
        {
            try
            {
                var tenant = await _repositoryWrapper.Tenant.GetTenantById(id);
                if (tenant == null)
                {
                    _logger.LogError($"Not deleted. Tenant with given id: {id} was not found");
                    return NotFound();
                }
                else if (!tenant.DisplayOnWeb)
                {
                    _logger.LogError($"Not deleted. Tenant with given id: {id} must have been already deleted.");
                    return BadRequest($"Not deleted. Tenant with id: {id} must have already been deleted.");
                }

                // Change Tenant DisplayOnWeb to false
                tenant.DisplayOnWeb = false;
                var tenantEntity = _mapper.Map<Tenant>(tenant);
                _repositoryWrapper.Tenant.UpdateTenant(tenantEntity);

                await _repositoryWrapper.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside DeleteTenant(tenant_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //DELETE api/tenant/{tenant_id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UnDeleteTenant(int id)
        {
            try
            {
                var tenant = await _repositoryWrapper.Tenant.GetTenantById(id);
                if (tenant == null)
                {
                    _logger.LogError($"Not deleted. Tenant with given id: {id} was not found");
                    return NotFound();
                }
                else if (tenant.DisplayOnWeb)
                {
                    _logger.LogError($"Not undeleted. Tenant with given id: {id} must have been already undeleted.");
                    return BadRequest($"Not undeleted. Tenant with id: {id} must have been already undeleted.");
                }

                // Change Tenant DisplayOnWeb to true
                tenant.DisplayOnWeb = true;
                var tenantEntity = _mapper.Map<Tenant>(tenant);
                _repositoryWrapper.Tenant.UpdateTenant(tenantEntity);

                await _repositoryWrapper.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside UnDeleteTenant(tenant_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }
        /// <summary>
        /// (Auth: Tenant) This action is assumed to be called by certain Tenant that wants to SofTDelete HIS OWN account.
        /// </summary>
        /// <param name="id">Tenant's own id [not aspUserId]</param>
        /// <returns>204 if success</returns>
        //DELETE api/tenant/{tenant_id}/rodo
        [Authorize(Policy = "Tenant")]
        [HttpDelete("{id}/rodo")]
        public async Task<IActionResult> DeleteTenantRodo(int id)
        {
            try
            {

                var accessToken = HttpContext.Request.Headers["Authorization"].ToString();
                var tenant = await _repositoryWrapper.Tenant.GetTenantWithAddressAndRentsByTenantId(id);
                //Collection<Invoice> Invoices = new Collection<Invoice>();
                var address = tenant.Address;

                if (tenant == null)
                {
                    _logger.LogError($"Not deleted. Tenant with given id: {id} was not found");
                    return NotFound();
                }

                AnonimizationResponse anonimizationResult = await _identityService.AnonimizeUserByEmailFromToken(accessToken);

                // Check if result has errors.
                if (anonimizationResult.Message != "User successfully anonimized" && anonimizationResult.Errors.Count() == 0)
                {
                    return BadRequest("User was not anonimized");
                }

                // Anonimize tenant Entity
                tenant.PESEL = "00000000000";
                tenant.NIP = "0000000000";
                tenant.REGON = "000000000";
                tenant.CompanyName = "RODO";
                tenant.PhoneNumber = "000000000";
                tenant.Name = "RODO";
                tenant.Surname = "RODO";
                tenant.LandlordComment = "Deleted due to Rodo";
                tenant.DisplayOnWeb = false;

                address.Street = "RODO";
                address.BuildingNumber = "RODO";
                address.FlatNumber = "RODO";
                address.PostalCode = "RODO";
                var tenantEntity = _mapper.Map<Tenant>(tenant);
                var addressEntity = _mapper.Map<Address>(address);
                _repositoryWrapper.Tenant.UpdateTenant(tenantEntity);
                _repositoryWrapper.Address.UpdateAddress(addressEntity);

                foreach (Rent rent in tenant.Rents)
                {
                    rent.Contract = new byte[2] {0,1};
                    _repositoryWrapper.Rent.UpdateRent(rent);
                }

                /*
                // Create an array of Invoice to be deleted (documents)
                foreach (Rent rent in tenant.Rents)
                {
                    IEnumerable<Invoice> singleRentInvoices = await _repositoryWrapper.Invoice.GetInvoicesByRentId(id);

                    foreach (Invoice i in singleRentInvoices)
                    {
                        Invoices.Add(i);
                    }
                }

                // Delete invoice documents that contain Tenant details
                foreach (Invoice invoice in Invoices)
                {
                    invoice.InvoiceDocument = new byte[2] { 0, 1 };
                    _repositoryWrapper.Invoice.UpdateInvoice(invoice);
                }
                */
                
                await _repositoryWrapper.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside DeleteTenantRodo(tenant_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

    }
}
 
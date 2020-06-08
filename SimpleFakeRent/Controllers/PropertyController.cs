using System;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Entities.DataTransferObjects.Property;
using System.Collections.Generic;
using System.Linq;
using SimpleFakeRent.Extensions;
using Microsoft.AspNetCore.Authorization;
using Entities.DataTransferObjects.Address;

namespace SimpleFakeRent.Controllers
{
    [Route("api/property")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public PropertyController(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        //GET api/property
        [HttpGet]
        public async Task<IActionResult> GetAllProperties()
        {
            try
            {
                var properties = await _repositoryWrapper.Property.GetAllProperties();

                if (properties == null)
                {
                    _logger.LogInfo($"No 'Property' has been found in db.");
                    return NotFound();
                }

                else
                {
                    _logger.LogInfo($"Returned all 'Properties' from db. Total number of Properties found: {properties.Count()}.");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyDto>>(properties);
                    return Ok(propertiesResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAllProperties() action: {e.Message}");
                _logger.LogError(e.ErrorMessageExtension());
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "Landlord,Tenant")]
        //GET api/property/id
        [HttpGet("{id}", Name = "PropertyById")]
        public async Task<IActionResult> GetPropertyById(int id)
        {
            try
            {
                bool userOwnsProperty = await _repositoryWrapper.Property.CheckIfUserOwnsProperty(HttpContext.SelectLoggedUserId(), id);
                if (!userOwnsProperty)
                {
                    return Unauthorized();
                }

                var property = await _repositoryWrapper.Property.GetPropertyById(id);

                if (property == null)
                {
                    _logger.LogError($"Property with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Property with id: {id}");
                    var propertyResult = _mapper.Map<PropertyDto>(property);
                    return Ok(propertyResult);
                }
                    
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertyById(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get property with related address by rent_id
        /// </summary>
        /// <param name="id">rent_id</param>
        /// <returns>PropertyWithAddressDto</returns>
        //GET api/property/id
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("rent/{id}", Name = "PropertyWithAddressByRentId")]
        public async Task<IActionResult> GetPropertyWithAddressByRentId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.Rent.GetRentWithPropertyAndAddress(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else if (rent.Property == null)
                {
                    _logger.LogError($"Property for rent with id: {id} hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Limited Property with id: {id}");
                    var propertyLimited = _mapper.Map<PropertyWithAddressDto>(rent.Property);
                    return Ok(propertyLimited);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertyWithAddressByRentId(rent_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get property with related address and rate by rent_id
        /// </summary>
        /// <param name="id">rent_id</param>
        /// <returns>PropertyWithAddressDto</returns>
        //GET api/property/id
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("rent/{id}/address/rate", Name = "PropertyWithAddressAndRateByRentId")]
        public async Task<IActionResult> GetPropertyWithRateAndAddressByRentId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.Rent.GetRentWithPropertyAndRateAndAddress(id);

                if (rent == null)
                {
                    _logger.LogError($"Rent with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else if (rent.Property == null)
                {
                    _logger.LogError($"Property for rent with id: {id} hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Limited Property with id: {id}");
                    var propertyLimited = _mapper.Map<PropertyWithAddressAndRateDto>(rent.Property);
                    return Ok(propertyLimited);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertyWithRateAndAddressByRentId(rent_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/id/limited
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/limited", Name = "PropertyByIdLimited")]
        public async Task<IActionResult> GetPropertyByIdLimited(int id)
        {
            try
            {
                var property = await _repositoryWrapper.Property.GetPropertyByIdLimited(id);

                if (property == null)
                {
                    _logger.LogError($"Property with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Property with id: {id}");
                    var propertyResult = _mapper.Map<PropertyLimitedDto>(property);
                    return Ok(propertyResult);
                }
                    
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertyByIdLimited(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/landlord/{landlord_id}
        [Authorize(Policy = "Landlord")]
        [HttpGet("landlord/{id}", Name = "PropertiesByLandlordId")]
        public async Task<IActionResult> GetPropertiesByLandlordId(int id)
        {
            try
            {
                var properties = await _repositoryWrapper.Property.GetPropertiesByLandlordId(id);

                if (properties == null)
                {
                    _logger.LogError($"Property with landlord_id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Property with landlord_id: {id}");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyDto>>(properties);
                    return Ok(propertiesResult);
                }
                    
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesByLandlordId(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/landlord/{landlord_id}/address
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/address", Name = "PropertiesWithAddressByLandlordId")]
        public async Task<IActionResult> GetPropertiesWithAddressByLandlordId(int id)
        {
            try
            {
                //bool userOwnsProperty = await _repositoryWrapper.Property.CheckIfUserOwnsProperty(HttpContext.SelectLoggedUserId(), id);
                if (HttpContext.SelectLoggedUserId()!=id)
                {
                    return Unauthorized();
                }
                var properties = await _repositoryWrapper.Property.GetPropertiesWithAddressByLandlordId(id);

                if (properties == null)
                {
                    _logger.LogError($"Property with its address, by landlord_id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Property with its address by landlord_id: {id}");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyWithAddressDto>>(properties);
                    return Ok(propertiesResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesWithAddressByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/landlord/{landlord_id}/rate
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/rate", Name = "PropertiesWithRateByLandlordId")]
        public async Task<IActionResult> GetPropertiesWithRateByLandlordId(int id)
        {
            try
            {
                bool userOwnsProperty = await _repositoryWrapper.Property.CheckIfUserOwnsProperty(HttpContext.SelectLoggedUserId(), id);
                if (!userOwnsProperty)
                {
                    return Unauthorized();
                }
                var properties = await _repositoryWrapper.Property.GetPropertiesWithRateByLandlordId(id);

                if (properties == null)
                {
                    _logger.LogError($"Property with its rate, by landlord_id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Property with its rate by landlord_id: {id}");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyWithRateDto>>(properties);
                    return Ok(propertiesResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesWithRateByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/landlord/{landlord_id}/rate/address
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/rate/address", Name = "PropertiesWithRateAndAddressByLandlordId")]
        public async Task<IActionResult> GetPropertiesWithRateAndAddressByLandlordId(int id)
        {
            try
            {
                var properties = await _repositoryWrapper.Property.GetPropertiesWithRateAndAddressByLandlordId(id);

                if (properties == null)
                {
                    _logger.LogError($"Property with its rate and address, by landlord_id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Property with its rate and address by landlord_id: {id}");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyWithRateAndAddressDto>>(properties);
                    return Ok(propertiesResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesWithRateAndAddressByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/{id}/address
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/address", Name = "PropertyWithAddressByPropertyId")]
        public async Task<IActionResult> GetPropertyWithAddress(int id)
        {
            try
            {
                bool userOwnsProperty = await _repositoryWrapper.Property.CheckIfUserOwnsProperty(HttpContext.SelectLoggedUserId(), id);
                if (!userOwnsProperty)
                {
                    return Unauthorized();
                }
                var property = await _repositoryWrapper.Property.GetPropertyWithAddress(id);

                if (property == null)
                {
                    _logger.LogError($"Property with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Property with id: {id} and its address.");
                    var propertyResult = _mapper.Map<PropertyWithAddressDto>(property);
                    return Ok(propertyResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertyWithAddress(property_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/{id}/rate
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/rate")]
        public async Task<IActionResult> GetPropertyWithRate(int id)
        {
            try
            {
                var property = await _repositoryWrapper.Property.GetPropertyWithRate(id);

                if (property == null)
                {
                    _logger.LogError($"Property with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Property with id: {id} and its rate.");
                    var propertyResult = _mapper.Map<PropertyWithRateDto>(property);
                    return Ok(propertyResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertyWithRate(property_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/{id}/rent
        [Authorize(Policy = "Landlord")]
        [HttpGet("{id}/rent")]
        public async Task<IActionResult> GetPropertyWithRents(int id)
        {
            try
            {
                var property = await _repositoryWrapper.Property.GetPropertyWithRents(id);

                if (property == null)
                {
                    _logger.LogError($"Property with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Property with id: {id} and its Rents, Rents count: {property.Rents.Count()}.");
                    var propertyResult = _mapper.Map<PropertyWithRentsDto>(property);
                    return Ok(propertyResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertyWithRents(property_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }


        //GET api/property/country/polska
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("country/{country}")]
        public async Task<IActionResult> GetPropertiesByCountry(string country)
        {
            try
            {
                var properties = await _repositoryWrapper.Property.GetPropertiesByCountry(country);

                if (properties == null)
                {
                    _logger.LogError($"Property with Country: {country}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Property with Country: {country}, properties found: {properties.Count()}.");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyDto>>(properties);
                    return Ok(propertiesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesByCountry(country) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/city/zakopane
        [Authorize(Policy = "Landlord")]
        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetPropertiesByCity(string city)
        {
            try
            {
                var properties = await _repositoryWrapper.Property.GetPropertiesByCity(city);

                if (properties == null)
                {
                    _logger.LogError($"Properties with city: {city}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Properties with city: {city}, number of properties found: {properties.Count()}.");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyDto>>(properties);
                    return Ok(propertiesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesByCity(city) action: {e.Message}.");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/street/pionkowa
        [Authorize(Policy = "Landlord")]
        [HttpGet("street/{street}")]
        public async Task<IActionResult> GetPropertiesByStreet(string street)
        {
            try
            {
                var properties = await _repositoryWrapper.Property.GetPropertiesByStreet(street);

                if (properties == null)
                {
                    _logger.LogError($"Property on street : {street}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Properties with street: {street}. Property with street count: {properties.Count()}.");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyDto>>(properties);
                    return Ok(propertiesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesByStreet(street) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/zip/80-265
        [Authorize(Policy = "Landlord")]
        [HttpGet("zip/{zipCode}")]
        public async Task<IActionResult> GetPropertiesByZipCode(string zipCode)
        {
            try
            {
                var properties = await _repositoryWrapper.Property.GetPropertiesByZipCode(zipCode);

                if (properties == null)
                {
                    _logger.LogError($"Property under ZIP : {zipCode}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Properties under ZIP: {zipCode}. Property count: {properties.Count()}.");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyDto>>(properties);
                    return Ok(propertiesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesByZipCode(ZIP) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/flatsize/53.5
        [Authorize(Policy = "Landlord")]
        [HttpGet("flatsize/{flatsize}")]
        public async Task<IActionResult> GetPropertiesByFlatSize(decimal flatsize)
        {
            try
            {
                var properties = await _repositoryWrapper.Property.GetPropertiesByFlatSize(flatsize);

                if (properties == null)
                {
                    _logger.LogError($"Property of size:: {flatsize}[m^2], hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Property with size: {flatsize}[m^2]. Number of properties with specified size: {properties.Count()}.");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyDto>>(properties);
                    return Ok(propertiesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesByFlatSize(property_size[m^2]) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/gas/true
        [Authorize(Policy = "Landlord")]
        [HttpGet("gas/{hasGas}")]
        public async Task<IActionResult> GetPropertiesByHasGas(bool hasGas)
        {
            try
            {
                var properties = await _repositoryWrapper.Property.GetPropertiesByHasGas(hasGas);

                if (properties == null)
                {
                    _logger.LogError($"Property has Gas: {hasGas}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Properties with field hasGas set to: {hasGas}. Properties with gas count: {properties.Count()}.");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyDto>>(properties);
                    return Ok(propertiesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesByHasGas(bool: hasGas) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/hotwater/true
        [Authorize(Policy = "Landlord")]
        [HttpGet("hotwater/{hasHotWater}")]
        public async Task<IActionResult> GetPropertiesByHasHotWater(bool hasHotWater)
        {
            try
            {
                var properties = await _repositoryWrapper.Property.GetPropertiesByHasHotWater(hasHotWater);

                if (properties == null)
                {
                    _logger.LogError($"Property 'hasHW': {hasHotWater}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Properties with field 'hasHW' set to: {hasHotWater}. Properties with hasHotWater {properties.Count()}.");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyDto>>(properties);
                    return Ok(propertiesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesByHasHotWater(bool: hasHW) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/property/heat/true
        [Authorize(Policy = "Landlord")]
        [HttpGet("heat/{hasHeat}")]
        public async Task<IActionResult> GetPropertiesByHasHeat(bool hasHeat)
        {
            try
            {
                var properties = await _repositoryWrapper.Property.GetPropertiesByHasHeat(hasHeat);

                if (properties == null)
                {
                    _logger.LogError($"Property 'hasHeat': {hasHeat}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned Properties with field 'hasHeat' set to: {hasHeat}. Properties with hasHeat {properties.Count()}.");
                    var propertiesResult = _mapper.Map<IEnumerable<PropertyDto>>(properties);
                    return Ok(propertiesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPropertiesByHasHeat(bool: hasHeat) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }


        //POST api/property
        [Authorize(Policy = "Landlord")]
        [HttpPost]
        //Implement mapper
        public async Task<IActionResult> CreateProperty([FromBody] PropertyForCreationDto property)
        {
            try
            {
                if (property == null)
                {
                    _logger.LogError("Property received is a Null Object.");
                    return BadRequest("Property object is null. Please send full request.");
                }
                else if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Property object sent from client.");
                    return BadRequest("Property object is not Valid");
                }
         
                var propertyEntity = _mapper.Map<Property>(property);
                
                _repositoryWrapper.Property.CreateProperty(propertyEntity);
                await _repositoryWrapper.Save();

                var createdProperty = _mapper.Map<PropertyWithAddressDto>(propertyEntity);
                var address = await _repositoryWrapper.Address.GetAddressById(createdProperty.AddressId);
                createdProperty.Address = _mapper.Map<AddressDto>(address);
                return CreatedAtRoute("PropertyWithAddressByPropertyId", new { id = createdProperty.Id }, createdProperty);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside CreateProperty(propertForCreationDto) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //PUT api/property
        [Authorize(Policy = "Landlord")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(int id, [FromBody]PropertyForUpdateDto property)
        {
            try
            {
                bool userOwnsProperty = await _repositoryWrapper.Property.CheckIfUserOwnsProperty(HttpContext.SelectLoggedUserId(), id);
                if (!userOwnsProperty)
                {
                    return Unauthorized();
                }

                if (property == null)
                {
                    _logger.LogError("Property received is a Null Object.");
                    return BadRequest("Property object is null. Please send full request.");
                }
                else if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Property object sent from client.");
                    return BadRequest("Property object is not Valid");
                }

                var propertyEntity = await _repositoryWrapper.Property.GetPropertyById(id);

                // Check if it exist before updating anything.
                if (propertyEntity == null)
                {
                    _logger.LogError($"Property with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(property, propertyEntity);
                _repositoryWrapper.Property.UpdateProperty(propertyEntity);

                await _repositoryWrapper.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside UpdateProperty(id, property) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //DELETE api/property
        [Authorize(Policy = "Landlord")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            try
            {
                bool userOwnsProperty = await _repositoryWrapper.Property.CheckIfUserOwnsProperty(HttpContext.SelectLoggedUserId(), id);
                if (!userOwnsProperty)
                {
                    return Unauthorized();
                }
                var property = await _repositoryWrapper.Property.GetPropertyWithRentsAndAddress(id);
                if (property == null)
                {
                    _logger.LogError($"Not deleted. Property with given id: {id} was not found");
                    return NotFound();
                }
                else if (!property.DisplayOnWeb)
                {
                    _logger.LogError($"Not deleted. Property with given id: {id} has already been deleted before.");
                    return BadRequest($"Not deleted. Property with id: {id} has already been deleted before.");
                }

                // Change Property DisplayOnWeb to false
                property.DisplayOnWeb = false;
                var propertyEntity = _mapper.Map<Property>(property);
                _repositoryWrapper.Property.UpdateProperty(propertyEntity);

                // Change Rents related to Property DisplayOnWeb to false
                foreach (Rent rent in property.Rents)
                {
                    rent.DisplayOnWeb = false;
                    _repositoryWrapper.Rent.UpdateRent(rent);
                }

                await _repositoryWrapper.Save();

                /*_repositoryWrapper.Property.DeleteProperty(property);
                _repositoryWrapper.Address.DeleteAddress(property.Address);
                await _repositoryWrapper.Save();*/

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside DeleteProperty(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //Patch api/property
        [Authorize(Policy = "Landlord")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UnDeleteProperty(int id)
        {
            try
            {
                bool userOwnsProperty = await _repositoryWrapper.Property.CheckIfUserOwnsProperty(HttpContext.SelectLoggedUserId(), id);
                if (!userOwnsProperty)
                {
                    return Unauthorized();
                }
                var property = await _repositoryWrapper.Property.GetPropertyWithRentsAndAddress(id);
                if (property == null)
                {
                    _logger.LogError($"Not undeleted. Property with given id: {id} was not found");
                    return NotFound();
                }
                else if (property.DisplayOnWeb)
                {
                    _logger.LogError($"Not undeleted. Property with given id: {id} has already been undeleted before.");
                    return BadRequest($"Not undeleted. Property with id: {id} has already been undeleted before.");
                }

                // Change Property DisplayOnWeb back to true
                property.DisplayOnWeb = true;
                var propertyEntity = _mapper.Map<Property>(property);
                _repositoryWrapper.Property.UpdateProperty(propertyEntity);

                // Change Rents related to Property DisplayOnWeb back to true
                foreach (Rent rent in property.Rents)
                {
                    rent.DisplayOnWeb = true;
                    _repositoryWrapper.Rent.UpdateRent(rent);
                }

                await _repositoryWrapper.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside UnDeleteProperty(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }


    }
}
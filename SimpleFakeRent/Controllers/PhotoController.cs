using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.Photo;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleFakeRent.Controllers
{
    [Route("api/photo")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public PhotoController(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }


        //GET api/photo/{photo_id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}", Name = "PhotoById")]
        public async Task<IActionResult> GetPhotoById(int id)
        {
            try
            {
                var photo = await _repositoryWrapper.Photo.GetPhotoById(id);

                if (photo == null)
                {
                    _logger.LogError($"Photo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned table Photo, full one row with id: {id}");
                    var photoResult = _mapper.Map<PhotoDto>(photo);
                    return Ok(photoResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPhotoById(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/photo/state/{state_id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("state/{id}", Name = "PhotoByStateId")]
        public async Task<IActionResult> GetPhotoByStateId(int id)
        {
            try
            {
                var photo = await _repositoryWrapper.Photo.GetPhotoByStateId(id);

                if (photo == null)
                {
                    _logger.LogError($"Photo with state_id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned table Photo, full one row with id: {id}");
                    var photoResult = _mapper.Map<PhotoDto>(photo);
                    return Ok(photoResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPhotoByStateId(id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/photo/{photo_id}/type/{coldwater} || {hotwater} || {gas} || {energy} || {heat}
        // Returns just photo, no other details (fastest)
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/type/{photoColumn}")]
        public async Task<IActionResult> GetPhotoByIdAndAnyPhotoColumnName(int id, string photoColumn)
        {
            try
            {
                var photo = await _repositoryWrapper.Photo.GetPhotoByIdOneColumn(id, photoColumn);
                if (photo == null)
                {
                    _logger.LogError($"Photo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else if (photo.Length == 0)
                {
                    _logger.LogError($"Photo with id: {id}, hasn't been found in db.");
                    return NotFound("Wrong column name");
                }
                else
                {
                    _logger.LogInfo($"Returned single Photo, no other details, of PhotoId: {id}");
                    return Ok(photo); 
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPhotoByIdAndAnyPhotoColumnName(photo_id, photo_column_name) action: {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

        //GET api/photo/{photo_id}/coldwater
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/coldwater")]
        public async Task<IActionResult> GetPhotoByIdOnlyColdWater(int id)
        {
            try
            {
                var photo = await _repositoryWrapper.Photo.GetPhotoById(id);
                if (photo == null)
                {
                    _logger.LogError($"Photo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned row Photo, just ColdWater Photo with id: {id}");
                    var photoResult = _mapper.Map<PhotoColdWaterDto>(photo);
                    return Ok(photoResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPhotoByIdOnlyColdWater(photo_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

        //GET api/photo/{photo_id}/hotwater
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/hotwater")]
        public async Task<IActionResult> GetPhotoByIdOnlyHotWater(int id)
        {
            try
            {
                var photo = await _repositoryWrapper.Photo.GetPhotoById(id);
                if (photo == null)
                {
                    _logger.LogError($"Photo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned row Photo, just HotWater Photo with id: {id}");
                    var photoResult = _mapper.Map<PhotoHotWaterDto>(photo);
                    return Ok(photoResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPhotoByIdOnlyHotWater(photo_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

        //GET api/photo/{photo_id}/gas
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/gas")]
        public async Task<IActionResult> GetPhotoByIdOnlyGas(int id)
        {
            try
            {
                var photo = await _repositoryWrapper.Photo.GetPhotoById(id);
                if (photo == null)
                {
                    _logger.LogError($"Photo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned row Photo, just Gas Photo with id: {id}");
                    var photoResult = _mapper.Map<PhotoGasDto>(photo);
                    return Ok(photoResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPhotoByIdOnlyGas(photo_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

        //GET api/photo/{photo_id}/energy
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/energy")]
        public async Task<IActionResult> GetPhotoByIdOnlyEnergy(int id)
        {
            try
            {
                var photo = await _repositoryWrapper.Photo.GetPhotoById(id);
                if (photo == null)
                {
                    _logger.LogError($"Photo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned row Photo, just Energy Photo with id: {id}");
                    var photoResult = _mapper.Map<PhotoEnergyDto>(photo);
                    return Ok(photoResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPhotoByIdOnlyEnergy(photo_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

        //GET api/photo/{photo_id}/heat
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/heat")]
        public async Task<IActionResult> GetPhotoByIdOnlyHeat(int id)
        {
            try
            {
                var photo = await _repositoryWrapper.Photo.GetPhotoById(id);
                if (photo == null)
                {
                    _logger.LogError($"Photo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned row Photo, just Heat Photo with id: {id}");
                    var photoResult = _mapper.Map<PhotoHeatDto>(photo);
                    return Ok(photoResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPhotoByIdOnlyHeat(photo_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }

        }

        //POST api/photo
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpPost]
        public async Task<IActionResult> CreatePhoto([FromBody] PhotoForCreationDto photo)
        {
            try
            {
                if (photo == null)
                {
                    _logger.LogError("Photo received is a Null Object.");
                    return BadRequest("Photo object is null. Please send full request.");
                }
                else if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Photo object sent from client.");
                    return BadRequest("Photo object is not Valid");
                }

                var RelatedState = await _repositoryWrapper.State.GetStateById(photo.StateId);
                var GivenStateIdPhoto = await _repositoryWrapper.Photo.GetPhotoByStateId(photo.StateId);
                if (RelatedState.IsInitial)
                {
                    _logger.LogError("Photo received was related to an Initial State.");
                    return BadRequest($"Photo cannot be created. State with id: {photo.StateId} is Initial and should not have Photo.");
                }
                else if (!RelatedState.IsInitial && GivenStateIdPhoto != null)
                {
                    _logger.LogError("Photo creation request cannot be fulfilled. Such State already has Photo in DB");
                    return BadRequest($"Photo cannot be created. State with id: {photo.StateId} already has Photo row.");
                }

                var photoEntity = _mapper.Map<Photo>(photo);

                _repositoryWrapper.Photo.CreatePhoto(photoEntity);
                await _repositoryWrapper.Save();

                var createPhoto = _mapper.Map<PhotoDto>(photoEntity);
                return CreatedAtRoute("PhotoById", new { id = createPhoto.Id }, createPhoto);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside CreatePhoto(photoForCreationDto) action: {e.Message}");
                return StatusCode(500, e.ToString());
            }
        }

        //PUT api/photo/{id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePhoto(int id, [FromBody]PhotoForUpdateDto photo)
        {
            try
            {
                if (photo == null)
                {
                    _logger.LogError("Photo received is a Null Object.");
                    return BadRequest("Photo object is null. Please send full request.");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Photo object sent from client.");
                    return BadRequest("Photo object is not Valid");
                }

                var photoEntity = await _repositoryWrapper.Photo.GetPhotoById(id);
                if (photoEntity == null)
                {
                    _logger.LogError($"Photo with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(photo, photoEntity);
                _repositoryWrapper.Photo.UpdatePhoto(photoEntity);

                await _repositoryWrapper.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside UpdatePhoto(id, photoForUpdateDto) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }



        //DELETE api/photo
        [Authorize(Policy = "Landlord")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            try
            {
                var photo = await _repositoryWrapper.Photo.GetPhotoById(id);

                if (photo == null)
                {
                    _logger.LogError("Photo Id not found, cannot delete.");
                    return NotFound();
                }

                _repositoryWrapper.Photo.DeletePhoto(photo);
                await _repositoryWrapper.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside DeletePhoto(id) action: {e.Message}");
                return StatusCode(500, e.ToString());
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.State;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleFakeRent.Extensions;

namespace SimpleFakeRent.Controllers
{
    [Route("api/state")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public StateController(IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        //GET api/state
        [HttpGet(Name = "States")]
        public async Task<IActionResult> GetAllStates()
        {
            try
            {
                var states = await _repositoryWrapper.State.GetAllStates();

                if (states == null)
                {
                    _logger.LogError($"No states found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned all rows from table State.");
                    var statesResult = _mapper.Map<IEnumerable<StateDto>>(states);
                    return Ok(statesResult);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetAllStates() action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/state/{state_id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}", Name = "StateById")]
        public async Task<IActionResult> GetStateById(int id)
        {
            try
            {
                var state = await _repositoryWrapper.State.GetStateById(id);

                if (state == null)
                {
                    _logger.LogError($"State with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned State with id: {id}");
                    var stateResult = _mapper.Map<StateDto>(state);
                    return Ok(stateResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetStateById(state_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/state/{state_id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/previous", Name = "PreviousStateByCurrentStateId")]
        public async Task<IActionResult> GetPreviousStateByCurrentStateId(int id)
        {
            try
            {
                // Get current state to fetch all states by its: rent_id
                var state = await _repositoryWrapper.State.GetStateById(id);

                if (state == null)
                {
                    _logger.LogError($"State with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else // current state found
                {
                    // fetch states for this rent_id
                    var states = await _repositoryWrapper.State.GetStatesByRentId(state.RentId);
                    List<State> statesList = new List<State>();
                    statesList = states.ToList();
                    State previousState = null;

                    // sprawdź czy udalo się znaleźć stanów więcej niż 1, inaczej zwróć błąd
                    if (statesList.Count == 1)
                    {
                        _logger.LogError($"Rent contains only one state, there is no previous state.");
                        return NotFound();
                    }
                    _logger.LogError($"{statesList.Count}");
                    _logger.LogError($"{states.Count()}");

                    // compare states one by one and choose best fitting one
                    for (int i = 0; i < statesList.Count(); i++)
                    {
                        if (statesList[i].Id == state.Id)
                        {
                            if(i == 0)
                            {
                                return BadRequest("This must be initial state.");
                            }
                            else if(statesList[i - 1].ColdWaterState <= state.ColdWaterState)
                            {
                                previousState = statesList[i-1];
                            }
                        }
                    }

                    _logger.LogInfo($"Returned State with id: {id}");
                    var stateResult = _mapper.Map<StateDto>(previousState);
                    return Ok(stateResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetPreviousStateByCurrentStateId(state_id) action: {e.ToString()}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/state/{state_id}/confirm
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/confirm", Name = "ConfirmStateById")]
        public async Task<IActionResult> ConfirmStateByStateId(int id)
        {
            try
            {
                var state = await _repositoryWrapper.State.GetStateById(id);

                if (state == null)
                {
                    _logger.LogError($"State with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Confirmed State with id: {id}");
                    state.IsConfirmed = true;

                    _repositoryWrapper.State.UpdateState(state);
                    await _repositoryWrapper.Save();

                    return NoContent();
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside ConfirmStateByStateId(state_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/state/{state_id}/unconfirm
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/unconfirm", Name = "UnconfirmStateById")]
        public async Task<IActionResult> UnconfirmStateByStateId(int id)
        {
            try
            {
                var state = await _repositoryWrapper.State.GetStateById(id);

                if (state == null)
                {
                    _logger.LogError($"State with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Uncofnirmed State with id: {id}");
                    state.IsConfirmed = false;

                    _repositoryWrapper.State.UpdateState(state);
                    await _repositoryWrapper.Save();

                    return NoContent();
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside UnconfirmStateByStateId(state_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/state/rent/{rate_id}
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("rent/{id}", Name = "StatesByRentId")]
        public async Task<IActionResult> GetStatesByRentId(int id)
        {
            try
            {
                //////////////////////////////////////////////////
                bool userOwnsState = await _repositoryWrapper.Rent.UserOwnsRent(HttpContext.SelectLoggedUserId(), id);
                if (!userOwnsState)
                {
                    return Unauthorized();
                }
                ///////////////////////////////////////////////////
                var rents = await _repositoryWrapper.State.GetStatesByRentId(id);

                if (rents == null)
                {
                    _logger.LogError($"State with RentId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned States with rent_id: {id}, States by Rent_id count: {rents.Count()}");
                    var rentsResult = _mapper.Map<IEnumerable<StateDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetStatesByRentId(rent_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/state/rent/{rate_id}/unconfirmed
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("rent/{id}/unconfirmed", Name = "UnconfirmedStatesByRentId")]
        public async Task<IActionResult> GetUnconfirmedStatesByRentId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.State.GetUnconfirmedStatesByRentId(id);

                if (rents == null)
                {
                    _logger.LogError($"Unconfirmed states with RentId: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned unconfirmed States by rent_id: {id}, States by Rent_id count: {rents.Count()}");
                    var rentsResult = _mapper.Map<IEnumerable<StateDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetUnconfirmedStatesByRentId(rent_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }


        //GET api/state/{id}/invoice
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/invoice", Name = "StateWithInvoiceByStateId")]
        public async Task<IActionResult> GetStateWithInvoiceById(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.State.GetStateWithInvoiceByStateId(id);

                if (rent == null)
                {
                    _logger.LogError($"State with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned State with state_id: {id} with related Invoice.");
                    var rentResult = _mapper.Map<StateWithInvoiceDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetStateWithInvoiceById(state_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/state/rent/{id}/invoice
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("rent/{id}/invoice", Name = "StatesWithInvoiceByRentId")]
        public async Task<IActionResult> GetStatesWithInvoiceByRentId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.State.GetStatesWithInvoiceByRentId(id);

                if (rents == null)
                {
                    _logger.LogError($"State with rent_id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned States with RentId: {id} with related Invoice.");
                    var rentsResult = _mapper.Map<IEnumerable<StatesWithInvoiceDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside StatesWithInvoiceByRentId(rent_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get states that are not yet confiremd by landlord
        /// </summary>
        /// <param name="id">landlord_id</param>
        /// <returns>state[]</returns>
        //GET api/state/landlord/{landlord_id}/unconfirmed
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("landlord/{id}/unconfirmed", Name = "StatesUnconfirmedByLandlordId")]
        public async Task<IActionResult> GetUnconfirmedStatesByLandlordId(int id)
        {
            try
            {
                ///////////////////////////////////////////
                if(HttpContext.SelectLoggedUserId() != id)
                {
                    return Unauthorized();
                }
                /////////////////////////////////////////
                IEnumerable<Rent> rents = await _repositoryWrapper.Rent.GetUnconfirmedStatesByLandlordId(id);

                if (rents == null)
                {
                    _logger.LogError($"Unconfirmed states for landlord with id: {id}, have not been found in db.");
                    return NotFound();
                }
                else
                {
                    List<State> statesUnconfirmed = new List<State>();
                    foreach (Rent rent in rents)
                    {
                        List<State> statesSelectedFalse = new List<State>();
                        foreach (State state in rent.States)
                        {
                            if(state.IsConfirmed == false)
                            {
                                statesSelectedFalse.Add(state);
                            }
                        }
                        statesUnconfirmed.AddRange(statesSelectedFalse);
                    }


                    _logger.LogInfo($"Returned States that are unconfirmed.");
                    var statesResult = _mapper.Map<IEnumerable<StateDto>>(statesUnconfirmed);
                    return Ok(statesResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetUnconfirmedStatesByLandlordId(landlord_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/state/{id}/photo
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/photo", Name = "StateWithPhotoByStateId")]
        public async Task<IActionResult> GetStateWithPhotoByStateId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.State.GetStateWithPhotoByStateId(id);

                if (rent == null)
                {
                    _logger.LogError($"State with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned State with state_id: {id} with related Photo.");
                    var rentResult = _mapper.Map<StateWithPhotoDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetStateWithPhotoByStateId(state_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/state/rent/{id}/photo
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("rent/{id}/photo", Name = "StatesWithPhotoByRentId")]
        public async Task<IActionResult> GetStatesWithPhotoByRentId(int id)
        {
            try
            {
                var rents = await _repositoryWrapper.State.GetStatesWithPhotoByRentId(id);

                if (rents == null)
                {
                    _logger.LogError($"State with rent_id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned States with RentId: {id} with related Photo.");
                    var rentsResult = _mapper.Map<IEnumerable<StatesWithPhotoDto>>(rents);
                    return Ok(rentsResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetStatesWithPhotoByRentId(rent_id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/state/{id}/rent
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/rent", Name = "StateWithRentByStateId")]
        public async Task<IActionResult> GetStateWithRentByStateId(int id)
        {
            try
            {
                /////////////////////////////////////////////////
                int? rentId = await _repositoryWrapper.State.GetRentIdByStateId(id);
                bool userOwnsState = await _repositoryWrapper.Rent.UserOwnsRent(HttpContext.SelectLoggedUserId(), rentId);
                if (rentId != null && !userOwnsState)
                {
                    return Unauthorized();
                }
                ///////////////////////////////////////////////
                var rent = await _repositoryWrapper.State.GetStateWithRentByStateId(id);

                if (rent == null)
                {
                    _logger.LogError($"State with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned State with state_id: {id} with related Rent.");
                    var rentResult = _mapper.Map<StateWithRentDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetStateWithRentByStateId(state_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //GET api/state/{id}/invoice/photo
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpGet("{id}/invoice/photo", Name = "StateWithInvoiceAndPhotoByStateId")]
        public async Task<IActionResult> GetStateWithInvoiceAndPhotoByStateId(int id)
        {
            try
            {
                var rent = await _repositoryWrapper.State.GetStateWithInvoiceAndPhotoByStateId(id);

                if (rent == null)
                {
                    _logger.LogError($"State with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned State with state_id: {id} with related Rent.");
                    var rentResult = _mapper.Map<StateWithInvoiceAndPhotoDto>(rent);
                    return Ok(rentResult);
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside GetStateWithRentByStateId(state_Id) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //POST api/state
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpPost]
        public async Task<IActionResult> CreateState([FromBody] StateForCreationDto state)
        {
            try
            {

                if (state == null)
                {
                    _logger.LogError("State received is a Null Object.");
                    return BadRequest("Rent object is null. Please send full request.");
                }
                else if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid State object sent from client.");
                    return BadRequest("State object is not Valid");
                }

                var relatedRentExist = await _repositoryWrapper.Rent.GetRentById(state.RentId);
                if(relatedRentExist == null)
                {
                    return BadRequest($"State cannot be created, Rent with id: {state.RentId} does not exist in DB.");
                }

                Boolean IsInitialExists = await _repositoryWrapper.State.CheckIsInitialStateWithRentIdExist(state.RentId);
                if (IsInitialExists && state.IsInitial)
                {
                    _logger.LogError("User tried to create 'Initial State', but 'Initial State' already exist in DB");
                    return BadRequest($"Initial State cannot be created, Rent with id: {state.RentId} already has its Initial State.");
                }
                else if (!IsInitialExists && !state.IsInitial)
                {
                    _logger.LogError("User tried to create 'Non Initial State', but 'Initial State' not yet exist in DB.");
                    return BadRequest($"Non Initial State cannot be created, Rent with id: {state.RentId} doesn't have it's initial state. Contact Landlord to create it first.");
                }

                var stateEntity = _mapper.Map<State>(state);

                _repositoryWrapper.State.CreateState(stateEntity);
                await _repositoryWrapper.Save();

                var createdState = _mapper.Map<StateDto>(stateEntity);
                return CreatedAtRoute("RentById", new { id = createdState.Id }, createdState);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside CreateState(StateForCreationDto) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //PUT api/state
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateState(int id, [FromBody]StateForUpdateDto state)
        {
            try
            {
                ///////////////////////////////////////////////////////
                bool userOwnsState = await _repositoryWrapper.Rent.UserOwnsRent(HttpContext.SelectLoggedUserId(), state.RentId);
                if (!userOwnsState)
                {
                    return Unauthorized();
                }
                ///////////////////////////////////////////////////////
                if (state == null)
                {
                    _logger.LogError("State received is a Null Object.");
                    return BadRequest("State object is null. Please send full request.");
                }
                else if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid State object sent from client.");
                    return BadRequest("State object is not Valid");
                }

                Boolean relatedRentExist = await _repositoryWrapper.Rent.CheckIfRentExistByRentId(state.RentId);
                if (!relatedRentExist)
                {
                    return BadRequest($"State cannot be updated, Rent with id: {state.RentId} does not exist in DB.");
                }

                Boolean stateExist = await _repositoryWrapper.State.CheckIfStateExistByStateId(id);
                if (!stateExist)
                {
                    _logger.LogError($"State with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var stateEntityFound = await _repositoryWrapper.State.GetStateById(id);
                _mapper.Map(state, stateEntityFound);
                _repositoryWrapper.State.UpdateState(stateEntityFound);

                await _repositoryWrapper.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside UpdateState(id, StateForUpdateDto) action: {e.Message}");
                return StatusCode(500, e.Message);
            }
        }

        //DELETE api/rent
        [Authorize(Policy = "Landlord")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            try
            {
                // Get State by Id with Invoice & Photo to check if they are constrained before delete action.
                var state = await _repositoryWrapper.State.GetStateWithInvoiceAndPhotoByStateId(id);

                if (state == null)
                {
                    _logger.LogError("State Id not found, cannot delete.");
                    return BadRequest("State object is null. Please send full request.");
                }
                else if (state.IsInitial)
                {
                    return BadRequest("State is initial, cannot delete, please update instead.");
                }
                else if (!state.IsInitial && state.Invoice != null)
                {
                    if (state.Invoice.IsDistributed)
                    { 
                        return BadRequest("State has related Invoice that was distributed to tenant, cannot delete.");
                    }
                }
                else if (!state.IsInitial && (state.Invoice != null || state.Photo != null))
                {
                    if (state.Invoice != null)
                    {
                        var invoiceEntityToBeDeleted = _mapper.Map<Invoice>(state.Invoice);
                        _repositoryWrapper.Invoice.DeleteInvoice(invoiceEntityToBeDeleted);
                    }
                    if (state.Photo != null)
                    {
                        var photoEntityToBeDeleted = _mapper.Map<Photo>(state.Photo);
                        _repositoryWrapper.Photo.DeletePhoto(photoEntityToBeDeleted);
                    }
                    await _repositoryWrapper.Save();
                    
                }

                _repositoryWrapper.State.DeleteState(state);
                await _repositoryWrapper.Save();

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong inside DeleteState(state_id) action: {e.Message}");
                return StatusCode(500, e.ToString());
            }
        }


    }
}
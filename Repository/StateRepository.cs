using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class StateRepository : RepositoryBase<State>, IStateRepository
    {
        public StateRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<State>> GetAllStates()
        {
            return await FindAll().OrderBy(s => s.Id)
                .OrderBy(s => s.ColdWaterState)
                .ToListAsync();
        }

        public async Task<State> GetStateById(int id)
        {
            return await FindByCondition(s => s.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<State>> GetStatesByRentId(int id)
        {
            return await FindByCondition(s => s.RentId.Equals(id))
                .OrderBy(s => s.ColdWaterState)
                .ToListAsync();
        }

        public async Task<IEnumerable<State>> GetStatesWithInvoiceByRentId(int id)
        {
            return await FindByCondition(s => s.RentId.Equals(id))
                .Include(s => s.Invoice)
                .OrderBy(s => s.ColdWaterState)
                .ToListAsync();
        }

        public async Task<IEnumerable<State>> GetUnconfirmedStatesByRentId(int id)
        {
            return await FindByCondition(s => s.RentId.Equals(id))
                .Where(s => s.IsConfirmed == false)
                .OrderBy(s => s.ColdWaterState)
                .ToListAsync();
        }

        public async Task<State> GetStateWithInvoiceByStateId(int id)
        {
            return await FindByCondition(s => s.Id.Equals(id))
                .Include(s => s.Invoice)
                .FirstOrDefaultAsync();
        }

        public async Task<State> GetStateWithPhotoByStateId(int id)
        {
            return await FindByCondition(s => s.Id.Equals(id)).Include(s => s.Photo).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<State>> GetStatesWithPhotoByRentId(int id)
        {
            return await FindByCondition(s => s.RentId.Equals(id))
                .Include(s => s.Photo)
                .OrderBy(s => s.ColdWaterState)
                .ToListAsync();
        }

        public async Task<State> GetStateWithRentByStateId(int id)
        {
            return await FindByCondition(s => s.Id.Equals(id)).Include(s => s.Rent).FirstOrDefaultAsync();
        }

        public async Task<State> GetStateWithInvoiceAndPhotoByStateId(int id)
        {
            return await FindByCondition(s => s.Id.Equals(id))
                .Include(s => s.Invoice)
                .Include(s => s.Photo)
                .FirstOrDefaultAsync();
        }

        public async Task<Boolean> CheckIsInitialStateWithRentIdExist(int id)
        {
            return await FindByCondition(s => s.RentId.Equals(id)).Where(s => s.IsInitial == true).AnyAsync();
        }
        public async Task<Boolean> CheckNotInitialStateWithRentIdExist(int id)
        {
            return await FindByCondition(s => s.RentId.Equals(id)).Where(s => s.IsInitial == false).AnyAsync();
        }

        public async Task<Boolean> CheckIfStateExistByStateId(int id)
        {
            return await FindByCondition(s => s.Id.Equals(id)).AnyAsync();
        }

        public void CreateState(State state)
        {
            Create(state);
        }

        public void UpdateState(State state)
        {
            Update(state);
        }

        public void DeleteState(State state)
        {
            Delete(state);
        }
        public async Task<int?> GetRentIdByStateId(int stateId)
        {
            return await FindByCondition(s => s.Id.Equals(stateId)).Select(x => x.RentId).FirstOrDefaultAsync();
        }
    }
}

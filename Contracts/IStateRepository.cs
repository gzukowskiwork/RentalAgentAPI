using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IStateRepository : IRepositoryBase<State>
    {
        Task<IEnumerable<State>> GetAllStates();
        Task<State> GetStateById(int id);
        Task<IEnumerable<State>> GetStatesByRentId(int id);
        Task<IEnumerable<State>> GetUnconfirmedStatesByRentId(int id);
        Task<State> GetStateWithInvoiceByStateId(int id);
        Task<IEnumerable<State>> GetStatesWithInvoiceByRentId(int id);
        Task<State> GetStateWithPhotoByStateId(int id);
        Task<IEnumerable<State>> GetStatesWithPhotoByRentId(int id);
        Task<State> GetStateWithRentByStateId(int id);
        Task<State> GetStateWithInvoiceAndPhotoByStateId(int id);
        Task<Boolean> CheckIsInitialStateWithRentIdExist(int id);
        Task<Boolean> CheckNotInitialStateWithRentIdExist(int id);
        Task<Boolean> CheckIfStateExistByStateId(int id);
        void CreateState(State state);
        void UpdateState(State state);
        void DeleteState(State state);
        Task<int?> GetRentIdByStateId(int stateId);

    }
}

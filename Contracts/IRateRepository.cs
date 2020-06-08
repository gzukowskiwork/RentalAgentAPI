using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRateRepository : IRepositoryBase<Rate>
    {
        Task<IEnumerable<Rate>> GetAllRates();
        Task<Rate> GetRateById(int id);
        Task<Rate> GetRateByPropertyId(int id);
        Task<Rate> GetRateWithProperty(int id);
        Task<Rate> GetRateWithPropertyByPropertyId(int id);
        Task<Rate> GetRateWithPropertyAndAddressByPropertyId(int id);
        void CreateRate(Rate rate);
        void UpdateRate(Rate rate);
        void DeleteRate(Rate rate);
    }
}

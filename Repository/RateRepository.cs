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
    public class RateRepository : RepositoryBase<Rate>, IRateRepository
    {
        public RateRepository(RepositoryContext repositoryContext): base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Rate>> GetAllRates()
        {
            return await FindAll().OrderBy(rate => rate.PropertyId).ToListAsync();
        }

        public async Task<Rate> GetRateById(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id) ).FirstOrDefaultAsync();
        }

        public async Task<Rate> GetRateByPropertyId(int id)
        {
            return await FindByCondition(r => r.PropertyId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Rate> GetRateWithProperty(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id)).Include(r => r.Property).FirstOrDefaultAsync();
        }

        public async Task<Rate> GetRateWithPropertyByPropertyId(int id)
        {
            return await FindByCondition(r => r.PropertyId.Equals(id))
                .Include(r => r.Property)
                .FirstOrDefaultAsync();
        }

        public async Task<Rate> GetRateWithPropertyAndAddressByPropertyId(int id)
        {
            return await FindByCondition(r => r.PropertyId.Equals(id))
                .Include(r => r.Property)
                .ThenInclude(p => p.Address)
                .FirstOrDefaultAsync();
        }

        public void CreateRate(Rate rate)
        {
            Create(rate);
        }

        public void UpdateRate(Rate rate)
        {
            Update(rate);
        }

        public void DeleteRate(Rate rate)
        {
            Delete(rate);
        }
    }
}

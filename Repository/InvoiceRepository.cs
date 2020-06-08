using Microsoft.EntityFrameworkCore;
using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class InvoiceRepository: RepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(RepositoryContext fakeRentContex): base(fakeRentContex)
        {

        }
        public async Task<IEnumerable<Invoice>> GetAllInvoices()
        {
            return await FindAll().OrderBy(l => l.Id).ToListAsync();
        }

        public async Task<Invoice> GetInvoiceById(int id)
        {
            return await FindByCondition(i => i.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByRentId(int id)
        {
            return await FindByCondition(i => i.RentId.Equals(id)).ToListAsync();
        }
        public async Task<IEnumerable<Invoice>> GetInvoicesWithStateByRentId(int id)
        {
            return await FindByCondition(i => i.RentId.Equals(id))
                .Include(i => i.State)
                .OrderBy(i => i.ColdWaterState)
                .ToListAsync();
        }

        public async Task<Invoice> GetInvoiceWithRentByInvoiceId (int id)
        {
            return await FindByCondition(i => i.Id.Equals(id)).Include(i => i.Rent).FirstOrDefaultAsync();
        }

        // NEW ACTION

        public async Task<Invoice> GetInvoiceWithStateRentAndPropertyAndAddressByInvoiceId(int id)
        {
            return await FindByCondition(i => i.Id.Equals(id))
                .Include(i => i.State)
                .Include(i => i.Rent)
                    .ThenInclude(r => r.Tenant)
                .Include(i => i.Rent)
                    .ThenInclude(r => r.Tenant)
                .Include(i => i.Rent)
                    .ThenInclude(r => r.Property)
                        .ThenInclude(p => p.Address)
                .OrderBy(i => i.ColdWaterState)
                .FirstOrDefaultAsync();
        }

        // new action END

        public async Task<Invoice> GetInvoiceByStateId(int id)
        {
            return await FindByCondition(i => i.StateId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Invoice> GetInvoiceWithStateByInvoiceId(int id)
        {
            return await FindByCondition(i => i.Id.Equals(id))
                .Include(i => i.State)
                .OrderBy(i => i.ColdWaterState)
                .FirstOrDefaultAsync();
        }

        public async Task<Invoice> GetInvoiceWihtAllDetailsForInvoiceGeneration(int id)
        {
            return await FindByCondition(i => i.Id.Equals(id))
                .Include(i => i.State)
                    .ThenInclude(s => s.Photo)
                .Include(i => i.Rent)
                    .ThenInclude(r => r.Tenant)
                        .ThenInclude(t => t.Address)
                .Include(i => i.Rent)
                    .ThenInclude(r => r.Tenant)
                        .ThenInclude(t => t.ApplicationUser)
                .Include(i => i.Rent)
                    .ThenInclude(r => r.Property)
                        .ThenInclude(p => p.Address)
                .Include(i => i.Rent)
                    .ThenInclude(r => r.Property)
                        .ThenInclude(p => p.Rate)
                .Include(i => i.Rent)
                    .ThenInclude(r => r.Landlord)
                        .ThenInclude(l => l.Address)
                .Include(i => i.Rent)
                    .ThenInclude(r => r.Landlord)
                        .ThenInclude(l => l.ApplicationUser)
                .FirstOrDefaultAsync();
        }

        public async Task<Boolean> CheckIfInvoiceExistByStateId(int id)
        {
            return await FindByCondition(i => i.StateId.Equals(id)).AnyAsync();
        }

        public void CreateInvoice(Invoice invoice)
        {
            Create(invoice);
        }

        public void UpdateInvoice(Invoice invoice)
        {
            Update(invoice);
        }

        public void DeleteInvoice(Invoice invoice)
        {
            Delete(invoice);
        }
    }
}

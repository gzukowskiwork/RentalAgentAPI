using Contracts;
using Entities;
using Entities.DataTransferObjects.Rent;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RentRepository : RepositoryBase<Rent>, IRentRepository
    {
        public RentRepository(RepositoryContext repositoryContext): base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Rent>> GetAllRents()
        {
            return await FindAll().OrderBy(rent => rent.Id).ToListAsync();
        }

        public async Task<Rent> GetRentById(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id)).OrderBy(r => r.Id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Rent>> GetRentsByTenantId(int id)
        {
            return await FindByCondition(r => r.TenantId.Equals(id)).OrderBy(r => r.Id).ToListAsync();
        }

        public async Task<IEnumerable<Rent>> GetRentsByPropertyId(int id)
        {
            return await FindByCondition(r => r.PropertyId.Equals(id)).OrderBy(r => r.Id).ToListAsync();
        }

        public async Task<IEnumerable<Rent>> GetUnconfirmedStatesByLandlordId(int id)
        {
            return await FindByCondition(r => r.LandlordId.Equals(id))
                .Include(r => r.States)
                .OrderBy(r => r.Id)
                .ToListAsync();   
        }

        public async Task<IEnumerable<Rent>> GetRentsByTenantIdBetweenDates(int id, DateTime start, DateTime end)
        {
            return await FindByCondition(r => r.TenantId.Equals(id))
                .Where(r => (r.StartRent >= start) && (r.EndRent <= end) )
                .OrderBy(r => r.StartRent)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rent>> GetRentsThatHaveEndDateBeforeActualDateByTenantId(int id)
        {
            DateTime today = DateTime.Now;

            return await FindByCondition(r => r.TenantId.Equals(id))
                .Where(r => (r.EndRent >= today) && (r.StartRent <= today)  )
                .OrderBy(r => r.StartRent)
                .ToListAsync();
        }
       
        public async Task<IEnumerable<Rent>> GetRentsThatHaveEndDateBeforeActualDateWithPropertyByTenantId(int id)
        {
            DateTime today = DateTime.Now;

            return await FindByCondition(r => r.TenantId.Equals(id))
                .Where(r => (r.EndRent >= today) && (r.StartRent <= today))
                .OrderBy(r => r.StartRent)
                .Include (r => r.Property)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rent>> GetRentsThatHaveEndDateBeforeActualDateWithPropertyAndAddressByTenantId(int id)
        {
            DateTime today = DateTime.Now;

            return await FindByCondition(r => r.TenantId.Equals(id))
                .Where(r => (r.EndRent >= today) && (r.StartRent <= today))
                .OrderBy(r => r.StartRent)
                .Include(r => r.Property)
                .ThenInclude(p => p.Address)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rent>> GetRentsThatHaveEndDateBeforeActualDateWithTenantAndPropertyWithAddressByLandlordId(int id)
        {
            DateTime today = DateTime.Now;

            return await FindByCondition(r => r.LandlordId.Equals(id))
                .Where(r => (r.EndRent >= today) && (r.StartRent <= today))
                .OrderBy(r => r.StartRent)
                .Include(r => r.Tenant)
                .Include(r => r.Property)
                .ThenInclude(p => p.Address)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rent>> GetFinishedRentsWithTenantAndPropertyWithAddressByLandlordId(int id)
        {
            DateTime today = DateTime.Now;

            return await FindByCondition(r => r.LandlordId.Equals(id))
                .Where(r => (r.EndRent < today))
                .OrderBy(r => r.StartRent)
                .Include(r => r.Tenant)
                .Include(r => r.Property)
                .ThenInclude(p => p.Address)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rent>> GetRentsWithTenantAndPropertyWithAddressByTenantId(int id)
        {
            return await FindByCondition(r => r.TenantId.Equals(id))
                .OrderBy(r => r.StartRent)
                .Include(r => r.Tenant)
                .Include(r => r.Property)
                .ThenInclude(p => p.Address)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rent>> GetRentsWithTenantAndPropertyWithAddressByLandlordId(int id)
        {
            return await FindByCondition(r => r.LandlordId.Equals(id))
                .OrderBy(r => r.StartRent)
                .Include(r=> r.Tenant)
                .Include(r => r.Property)
                .ThenInclude(p => p.Address)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rent>> GetRentsByTenantIdThatAreFinished(int id)
        {
            DateTime today = DateTime.Now;

            return await FindByCondition(r => r.TenantId.Equals(id))
                .Where(r => r.EndRent < today)
                .OrderBy(r => r.StartRent)
                .ToListAsync();
        }

        public async Task<Rent> GetRentWithProperty(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id))
                .Include(r => r.Property)
                .FirstOrDefaultAsync();
        }

        public async Task<Rent> GetRentWithPropertyAndAddress(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id))
                .Include(r => r.Property)
                .ThenInclude(p => p.Address)
                .FirstOrDefaultAsync();
        }

        public async Task<Rent> GetRentWithPropertyAndAddressAndRate(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id))
                .Include(r => r.Property)
                .ThenInclude(p => p.Address)
                .Include(r => r.Property.Rate)
                .FirstOrDefaultAsync();
        }

        public async Task<Rent> GetRentWithPropertyAndRateAndAddress(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id))
                .Include(r => r.Property.Rate)
                .Include(r => r.Property)
                .ThenInclude(p => p.Address)
                .FirstOrDefaultAsync();
        }

        public async Task<Rent> GetRentWithPropertyTenantAndAddress(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id))
                .Include(r => r.Property).ThenInclude(t => t.Address)
                .Include(r => r.Tenant).ThenInclude(t => t.Address)
                .FirstOrDefaultAsync();
        }

        public async Task<Rent> GetRentWithStates(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id))
                .Include(r => r.States)
                .FirstOrDefaultAsync();
        }

        public async Task<Rent> GetRentWithInvoices(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id))
                .Include(r => r.Invoices)
                .FirstOrDefaultAsync();
        }
        public async Task<Rent> GetRentWithLandlord(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id))
                .Include(r => r.Landlord)
                .FirstOrDefaultAsync();
        }

        public async Task<Rent> GetRentWithTenant(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id))
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync();
        }

        public async Task<Rent> CheckIfSuchRentAlreadyExist(RentForCreationDto rent)
        {
            return await FindByCondition(r => (
                                                r.PropertyId.Equals(rent.PropertyId) && 
                                                r.TenantId.Equals(rent.TenantId) && 
                                                r.LandlordId.Equals(rent.LandlordId) &&
                                                r.StartRent.Equals(rent.StartRent) &&
                                                r.EndRent.Equals(rent.EndRent) &&
                                                r.RentPurpose.Equals(rent.RentPurpose)
                                               ) 
                                         ).FirstOrDefaultAsync();
        }

        public async Task<Rent> GetRentWithInvoiceAndStates(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id))
                .Include(r => r.Invoices).Include(r => r.States)
                .FirstOrDefaultAsync();
        }

        public async Task<Boolean> CheckIfRentExistByRentId(int id)
        {
            return await FindByCondition(r => r.Id.Equals(id)).AnyAsync();
        }

        public async Task<Boolean> CheckIfRentRelatedToLandlordExistByLandlordId(int id)
        {
            return await FindByCondition(r => r.LandlordId.Equals(id)).AnyAsync();
        }

        public async Task<IEnumerable<Tenant>> GetAllTenantsByLandlordId(int id)
        {
            return await FindByCondition(r => (r.LandlordId.Equals(id)))
                .Select(r => r.Tenant)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<Tenant>> GetAllTenantsWithApplicationUserByLandlordId(int id)
        {
            return await FindByCondition(r => (r.LandlordId.Equals(id)))
                .Select(r => r.Tenant)
                .Distinct()
                .Include(t =>t.ApplicationUser)
                .ToListAsync();
        }

        /// <summary>
        /// Returns list of tenants by Landlord Id, which have not ended their rent Yet!
        /// </summary>
        /// <param name="id"> LandlordId</param>
        /// <returns></returns>
        public async Task<IEnumerable<Tenant>> GetActualTenantsByLandlordId(int id)
        {
            DateTime today = DateTime.Now;

            return await FindByCondition(r => (r.LandlordId.Equals(id)))
                .Where(r => (r.EndRent >= today) && (r.StartRent <= today))
                .Select(r => r.Tenant)
                .Distinct()
                .Include(t => t.Address)
                .ToListAsync();
        }

        public void CreateRent(Rent rent)
        {
            Create(rent);
        }

        public void UpdateRent(Rent rent)
        {
            Update(rent);
        }

        public void DeleteRent(Rent rent)
        {
            Delete(rent);
        }
        public async Task<bool> UserOwnsRent(int loggedUserId, int? rentId)
        {
            int? ownersId =await FindByCondition(r => r.Id.Equals(rentId)).Select(x => x.LandlordId).FirstOrDefaultAsync();
            if(ownersId == null)
            {
                return true;
            }
            if (ownersId == loggedUserId)
            {
                return true;
            }
            return false;
        }

        //public async Task<bool> UserOwnsRentByTenantId(int loggedUser, int tenantId)
        //{

        //}

        public async Task<int> FindLandlordIdByTenantId(int tenantId)
        {
            return await FindByCondition(r => r.TenantId.Equals(tenantId)).Select(x => x.LandlordId).FirstOrDefaultAsync();
        }
    }
}

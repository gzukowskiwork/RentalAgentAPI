using Microsoft.EntityFrameworkCore;
using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.DataTransferObjects.Tenant;

namespace Repository
{
    public class TenantRepository: RepositoryBase<Tenant>, ITenantRepository
    {
        public TenantRepository(RepositoryContext fakeRentContext) : base(fakeRentContext)
        {

        }

        public async Task<IEnumerable<Tenant>> GetAllTenants()
        {
            return await FindAll().OrderBy(t => t.Id).ToListAsync();
        }

        public async Task<Tenant> GetTenantById(int id)
        {
            return await FindByCondition(t => t.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Tenant>> GetTenantsBySurname(string surname)
        {
            return await FindByCondition(t => t.Surname.Equals(surname)).ToListAsync();
        }

        public async Task<IEnumerable<Tenant>> GetTenantsByCity(string city)
        {
            return await FindByCondition(t => t.Address.City.Equals(city)).ToListAsync();
        }

        public async Task<IEnumerable<Tenant>> GetTenantsByCountry(string country)
        {
            return await FindByCondition(t => t.Address.Country.Equals(country)).ToListAsync();
        }

        public async Task<Tenant> GetTenantByEmail(string email)
        {
            return await FindByCondition(t => t.ApplicationUser.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public async Task<Tenant> GetTenantByPhone(string PhoneNumber)
        {
            return await FindByCondition(t => t.PhoneNumber.Equals(PhoneNumber)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Tenant>> GetTenantsByNip(string nip)
        {
            return await FindByCondition(t => t.NIP.Equals(nip)).ToListAsync();
        }

        public async Task<IEnumerable<Tenant>> GetTenantsByPesel(string pesel)
        {
            return await FindByCondition(t => t.PESEL.Equals(pesel)).ToListAsync();
        }

        public async Task<IEnumerable<Tenant>> GetTenantsByIsCompany(bool isCompany)
        {
            return await FindByCondition(t => t.IsCompany.Equals(isCompany)).ToListAsync();
        }
        public async Task<IEnumerable<Tenant>> GetTenantsByCompanyName(string company)
        {
            return await FindByCondition(t => t.CompanyName.Equals(company)).ToListAsync();
        }
        public async Task<IEnumerable<Tenant>> GetTenantsByStreet(string street)
        {
            return await FindByCondition(t => t.Address.Street.Equals(street)).ToListAsync();
        }

        public async Task<IEnumerable<Tenant>> GetTenantsByZipCode(string zipCode)
        {
            return await FindByCondition(t => t.Address.PostalCode.Equals(zipCode)).ToListAsync();
        }

        public async Task<Tenant> GetTenantWithAddress(int id)
        {
            return await FindByCondition(t => t.Id.Equals(id)).Include(t => t.Address).FirstOrDefaultAsync();
        }

        public async Task<Tenant> GetTenantWithAddressAndRentsByTenantId(int id)
        {
            return await FindByCondition(t => t.Id.Equals(id))
                .Include(t => t.Address)
                .Include(t => t.Rents)
                .FirstOrDefaultAsync();
        }

        public async Task<Boolean> CheckIfTenantExistByTenantId(int id)
        {
            return await FindByCondition(t => t.Id.Equals(id)).AnyAsync();
        }

        public async Task<Boolean> CheckIfTenantExistByAddressId(int id)
        {
            return await FindByCondition(t => t.AddressId.Equals(id)).AnyAsync();
        }
   
        public async Task<bool> CheckIfTenantWithEmailExist(string email)
        {
            return await FindByCondition(t => t.ApplicationUser.Email.Equals(email)).AnyAsync();
        }

        public async Task<Tenant> GetTenantByAspNetUserId(int id)
        {
            return await FindByCondition(t => t.AspNetUsersId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckIfTenantWithAspNetUserIdExist(int id)
        {
            return await FindByCondition(t => t.AspNetUsersId.Equals(id)).AnyAsync();
        }

        public async Task<Tenant> GetTenantByTenantDetails(TenantForCheckDto tenant)
        {
            return await FindByCondition(t => t.PhoneNumber.Equals(tenant.PhoneNumber))
                .Where(t => (t.NIP.Equals(tenant.NIP)) || (t.PESEL.Equals(tenant.PESEL)))
                .FirstOrDefaultAsync();
        }

        public void CreateTenant(Tenant tenant)
        {
            Create(tenant);
        }

        public void UpdateTenant(Tenant tenant)
        {
            Update(tenant);
        }
        
        public void DeleteTenant(Tenant tenant)
        {
            DeleteTenant(tenant);
        }




    }
}

using Entities.DataTransferObjects.Tenant;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITenantRepository: IRepositoryBase<Tenant>
    {
        Task<IEnumerable<Tenant>> GetAllTenants();
        Task<Tenant> GetTenantById(int id);
        Task<Tenant> GetTenantWithAddress(int id);
        Task<Tenant> GetTenantWithAddressAndRentsByTenantId(int id);
        Task<Tenant> GetTenantByEmail(string email);
        Task<Tenant> GetTenantByPhone(string PhoneNumber);
        Task<IEnumerable<Tenant>> GetTenantsByNip(string nip);
        Task<IEnumerable<Tenant>> GetTenantsByPesel(string pesel);
        Task<IEnumerable<Tenant>> GetTenantsByIsCompany(bool isCompany);
        Task<IEnumerable<Tenant>> GetTenantsByCompanyName(string company);
        Task<IEnumerable<Tenant>> GetTenantsBySurname(string surname);
        Task<IEnumerable<Tenant>> GetTenantsByCity(string city);
        Task<IEnumerable<Tenant>> GetTenantsByStreet(string street);
        Task<IEnumerable<Tenant>> GetTenantsByZipCode(string zipCode);
        Task<IEnumerable<Tenant>> GetTenantsByCountry(string country);
        Task<Boolean> CheckIfTenantExistByTenantId(int id);
        Task<Boolean> CheckIfTenantExistByAddressId(int id);
        Task<bool> CheckIfTenantWithEmailExist(string email);
        Task<Tenant> GetTenantByAspNetUserId(int id);
        Task<bool> CheckIfTenantWithAspNetUserIdExist(int id);
        Task<Tenant> GetTenantByTenantDetails(TenantForCheckDto tenant);
        void CreateTenant(Tenant tenant);
        void UpdateTenant(Tenant tenant);
        void DeleteTenant(Tenant tenant);
        
    }
}

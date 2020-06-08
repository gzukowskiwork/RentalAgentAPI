using Entities.DataTransferObjects.Rent;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRentRepository : IRepositoryBase<Rent>
    {
        Task<IEnumerable<Rent>> GetAllRents();
        Task<Rent> GetRentById(int id);
        Task<IEnumerable<Rent>> GetRentsByTenantId(int id);
        Task<IEnumerable<Rent>> GetRentsByPropertyId(int id);
        Task<IEnumerable<Rent>> GetUnconfirmedStatesByLandlordId(int id);
        Task<IEnumerable<Rent>> GetRentsByTenantIdBetweenDates(int id, DateTime start, DateTime end);
        Task<IEnumerable<Rent>> GetRentsThatHaveEndDateBeforeActualDateByTenantId(int id);
        Task<IEnumerable<Rent>> GetRentsThatHaveEndDateBeforeActualDateWithPropertyByTenantId(int id);
        Task<IEnumerable<Rent>> GetRentsThatHaveEndDateBeforeActualDateWithPropertyAndAddressByTenantId(int id);
        Task<IEnumerable<Rent>> GetRentsThatHaveEndDateBeforeActualDateWithTenantAndPropertyWithAddressByLandlordId(int id);
        Task<IEnumerable<Rent>> GetFinishedRentsWithTenantAndPropertyWithAddressByLandlordId(int id);
        Task<IEnumerable<Rent>> GetRentsWithTenantAndPropertyWithAddressByLandlordId(int id);
        Task<IEnumerable<Rent>> GetRentsWithTenantAndPropertyWithAddressByTenantId(int id);
        Task<IEnumerable<Rent>> GetRentsByTenantIdThatAreFinished(int id);
        Task<Rent> GetRentWithProperty(int id);
        Task<Rent> GetRentWithPropertyAndAddress(int id);
        Task<Rent> GetRentWithPropertyAndAddressAndRate(int id);
        Task<Rent> GetRentWithPropertyAndRateAndAddress(int id);
        Task<Rent> GetRentWithStates(int id);
        Task<Rent> GetRentWithPropertyTenantAndAddress(int id);
        Task<Rent> GetRentWithInvoices(int id);
        Task<Rent> GetRentWithLandlord(int id);
        Task<Rent> GetRentWithTenant(int id);
        Task<Rent> CheckIfSuchRentAlreadyExist(RentForCreationDto rent);
        Task<Rent> GetRentWithInvoiceAndStates(int id);
        Task<Boolean> CheckIfRentExistByRentId(int id);
        Task<Boolean> CheckIfRentRelatedToLandlordExistByLandlordId(int id);
        Task<IEnumerable<Tenant>> GetAllTenantsByLandlordId(int id);
        Task<IEnumerable<Tenant>> GetAllTenantsWithApplicationUserByLandlordId(int id);
        Task<IEnumerable<Tenant>> GetActualTenantsByLandlordId(int id);
        void CreateRent(Rent rent);
        void UpdateRent(Rent rent);
        void DeleteRent(Rent rent);
        Task<bool> UserOwnsRent(int loggedUserId, int? rentId);
        Task<int> FindLandlordIdByTenantId(int tenantId);
    }
}

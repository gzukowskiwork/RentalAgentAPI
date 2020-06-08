using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ILandlordRepository : IRepositoryBase<Landlord>
    {
        Task<IEnumerable<Landlord>> GetAllLandlords();
        Task<Landlord> GetLandlordById(int id);
        Task<Landlord> GetLandlordWithProperties(int id);
        Task<Landlord> GetLandlordWithAddress(int id);
        Task<Landlord> GetLandlordByEmail(string email);
        Task<IEnumerable<Landlord>> GetLandlordsBySurname(string surname);
        Task<IEnumerable<Landlord>> GetLandlordsByCity(string city);
        Task<IEnumerable<Landlord>> GetLandlordsByStreet(string street);
        Task<IEnumerable<Landlord>> GetLandlordsByZipCode(string zipCode);
        Task<IEnumerable<Landlord>> GetLandlordsByCountry(string country);
        Task<IEnumerable<Landlord>> GetLandlordByCompanyName(string company);
        Task<IEnumerable<Landlord>> GetLandlordsByNip(string nip);
        Task<IEnumerable<Landlord>> GetLandlordsByPesel(string pesel);
        Task<IEnumerable<Landlord>> GetLandlordByRegon(string regon);
        Task<Boolean> CheckIfLandlordExistByLandlordId(int id);
        Task<Boolean> CheckIfLandlordWithEmailExist(string email);
        Task<Boolean> CheckIfLandlordExistByAddressId(int id);
        Task<Landlord> GetAllRelatedEntitiesByLandlordId(int id);
        Task<Landlord> GetLandlordByAspNetUserId(int id);
        Task<bool> CheckIfLandlordWithAspNetUserIdExist(int id);
        void CreateLandlord(Landlord landlord);
        void UpdateLandlord(Landlord landlord);
        void DeleteLandlord(Landlord landlord);
        Task<bool> CheckIfUserOwnsAddress(int loggedUserId, int landlordId);
        Task<int> FindLandlordIdByAddressId(int addressId);
    }
}

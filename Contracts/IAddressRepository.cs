using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAddressRepository : IRepositoryBase<Address>
    {
        Task<IEnumerable<Address>> GetAllAddresses();
        Task<Address> GetAddressById(int id);
        Task<Address> GetAddressWithLandlord(int id);
        Task<Address> GetAddressWithProperty(int id);
        Task<Address> GetAddressWithTenant(int id);
        Task<IEnumerable<Address>> GetAddressByCity(string cityName);
        Task<IEnumerable<Address>> GetAddressByStreet(string streetName);
        Task<IEnumerable<Address>> GetAddressByZipCode(string postalCode);
        Task<IEnumerable<Address>> GetAddressByCountry(string countryName);
        Task<bool> CheckIfAddressExistByAddressId(int id);
        void CreateAddress(Address address);
        void UpdateAddress(Address address);
        void DeleteAddress(Address address);
    }
}

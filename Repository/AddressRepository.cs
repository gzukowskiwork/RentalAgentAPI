using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class AddressRepository: RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(RepositoryContext fakeRentContext): base(fakeRentContext)
        {

        }

        public async Task<Address> GetAddressWithLandlord(int id)
        {
            return await FindByCondition(a => a.Id.Equals(id)).Include(l => l.Landlord).FirstOrDefaultAsync();
        }

        public async Task<Address> GetAddressWithProperty(int id)
        {
            return await FindByCondition(a => a.Id.Equals(id)).Include(l => l.Property).FirstOrDefaultAsync();
        }

        public async Task<Address> GetAddressWithTenant(int id)
        {
            return await FindByCondition(a => a.Id.Equals(id)).Include(l => l.Tenant).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Address>> GetAddressByCity(string cityName)
        {
            return await FindByCondition(a => a.City.Equals(cityName)).ToListAsync();
        }

        public async Task<IEnumerable<Address>> GetAddressByCountry(string countryName)
        {
            return await FindByCondition(a => a.Country.Equals(countryName)).ToListAsync();
        }

        public async Task<Address> GetAddressById(int id)
        {
            return await FindByCondition(a => a.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Address>> GetAddressByStreet(string streetName)
        {
            return await FindByCondition(a => a.Street.Equals(streetName)).ToListAsync();
        }

        public async Task<IEnumerable<Address>> GetAddressByZipCode(string postalCode)
        {
            return await FindByCondition(a => a.PostalCode.Equals(postalCode)).ToListAsync();
        }

        public async Task<IEnumerable<Address>> GetAllAddresses()
        {
            return await FindAll().OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<bool> CheckIfAddressExistByAddressId(int id)
        {
            return await FindByCondition(a => a.Id.Equals(id)).AnyAsync();
        }

        public void CreateAddress(Address address)
        {
            Create(address);
        }

        public void UpdateAddress(Address address)
        {
            Update(address);
        }

        public void DeleteAddress(Address address)
        {
            Delete(address);
        }

       
    }
}

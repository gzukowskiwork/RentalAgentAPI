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
    public class PropertyRepository : RepositoryBase<Property>, IPropertyRepository
    {
        public PropertyRepository(RepositoryContext fakeRentContext) : base(fakeRentContext)
        {

        }

        public async Task<IEnumerable<Property>> GetAllProperties()
        {
            return await FindAll().OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<Property> GetPropertyById(int id)
        {
            return await FindByCondition(p => p.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Property> GetPropertyByIdLimited(int id)
        {
            return await FindByCondition(p => p.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Property> GetPropertyWithAddress(int id)
        {
            return await FindByCondition(p => p.Id.Equals(id)).Include(p => p.Address).FirstOrDefaultAsync();
        }

        public async Task<Property> GetPropertyWithRate(int id)
        {
            return await FindByCondition(p => p.Id.Equals(id)).Include(p => p.Rate).FirstOrDefaultAsync();
        }

        public async Task<Property> GetPropertyWithLandlordAndAddress(int id)
        {
            return await FindByCondition(p => p.Id.Equals(id)).Include(p => p.Landlord).Include(p => p.Address).FirstOrDefaultAsync();
        }

        public async Task<Property> GetPropertyWithRentsAndAddress(int id)
        {
            return await FindByCondition(p => p.Id.Equals(id)).Include(p => p.Rents).Include(p => p.Address).FirstOrDefaultAsync();
        }

        public async Task<Property> GetPropertyWithRents(int id)
        {
            return await FindByCondition(p => p.Id.Equals(id)).Include(p => p.Rents).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesByLandlordId(int id)
        {
            return await FindByCondition(p => p.LandlordId.Equals(id)).ToListAsync();
        }
        public async Task<IEnumerable<Property>> GetPropertiesByCity(string city)
        {
            return await FindByCondition(p => p.Address.City.Equals(city)).ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesWithAddressByLandlordId(int id)
        {
            return await FindByCondition(p => p.LandlordId.Equals(id)).Include(p => p.Address).ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesWithRateByLandlordId(int id)
        {
            return await FindByCondition(p => p.LandlordId.Equals(id)).Include(p => p.Rate).ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesWithRateAndAddressByLandlordId(int id)
        {
            return await FindByCondition(p => p.LandlordId.Equals(id))
                .Include(p => p.Rate)
                .Include(p => p.Address)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesByCountry(string country)
        {
            return await FindByCondition(p => p.Address.Country.Equals(country)).ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesByFlatSize(decimal flatSize)
        {
            return await FindByCondition(p => p.FlatSize.Equals(flatSize)).ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesByHasGas(bool hasGas)
        {
            return await FindByCondition(p => p.HasGas.Equals(hasGas)).ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesByHasHotWater(bool hasHotWater)
        {
            return await FindByCondition(p => p.HasHW.Equals(hasHotWater)).ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesByHasHeat(bool hasHeat)
        {
            return await FindByCondition(p => p.HasHeat.Equals(hasHeat)).ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesByStreet(string street)
        {
            return await FindByCondition(p => p.Address.Street.Equals(street)).ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesByZipCode(string zipCode)
        {
            return await FindByCondition(p => p.Address.PostalCode.Equals(zipCode)).ToListAsync();
        }

        public async Task<Boolean> CheckIfPropertyExistByPropertyId(int id)
        {
            return await FindByCondition(p => p.Id.Equals(id)).AnyAsync();
        }

        public async Task<Boolean> CheckIfPropertyRelatedToLandlordExistByLandlordId(int id)
        {
            return await FindByCondition(p => p.LandlordId.Equals(id)).AnyAsync();
        }

        public async Task<Boolean> CheckIfPropertyExistByAddressId(int id)
        {
            return await FindByCondition(p => p.AddressId.Equals(id)).AnyAsync();
        }

        public void CreateProperty(Property property)
        {
            Create(property);
        }
        public void UpdateProperty(Property property)
        {
            Update(property);
        }

        public void DeleteProperty(Property property)
        {
            Delete(property);
        }

        public async Task<bool> CheckIfUserOwnsProperty(int loggedUserId, int propertId)
        {
            int? landlordFormPropertyId = await FindByCondition(p => p.Id.Equals(propertId)).Select(x=>x.LandlordId).FirstOrDefaultAsync();
            if (landlordFormPropertyId == null)
            {
                return true;
            }
            if (landlordFormPropertyId.Equals(loggedUserId))
            {
                return true;
            }
            return false;
        }
    }
}

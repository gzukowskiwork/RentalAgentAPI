using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPropertyRepository: IRepositoryBase<Property>
    {
        Task<IEnumerable<Property>> GetAllProperties();
        Task<Property> GetPropertyById(int id);
        Task<Property> GetPropertyByIdLimited(int id);
        Task<Property> GetPropertyWithAddress(int id);
        Task<Property> GetPropertyWithRate(int id);
        Task<Property> GetPropertyWithLandlordAndAddress(int id);
        Task<Property> GetPropertyWithRentsAndAddress(int id);
        Task<Property> GetPropertyWithRents(int id);
        Task<IEnumerable<Property>> GetPropertiesByLandlordId(int id);
        Task<IEnumerable<Property>> GetPropertiesWithAddressByLandlordId(int id);
        Task<IEnumerable<Property>> GetPropertiesWithRateByLandlordId(int id);
        Task<IEnumerable<Property>> GetPropertiesWithRateAndAddressByLandlordId(int id);
        Task<IEnumerable<Property>> GetPropertiesByCity(string city);
        Task<IEnumerable<Property>> GetPropertiesByStreet(string street);
        Task<IEnumerable<Property>> GetPropertiesByZipCode(string zipCode);
        Task<IEnumerable<Property>> GetPropertiesByCountry(string country);
        Task<IEnumerable<Property>> GetPropertiesByHasGas(bool hasGas);
        Task<IEnumerable<Property>> GetPropertiesByHasHotWater(bool hasHotWater);
        Task<IEnumerable<Property>> GetPropertiesByHasHeat(bool hasHeat);
        Task<IEnumerable<Property>> GetPropertiesByFlatSize(decimal flatSize);
        Task<Boolean> CheckIfPropertyExistByPropertyId(int id);
        Task<Boolean> CheckIfPropertyRelatedToLandlordExistByLandlordId(int id);
        Task<Boolean> CheckIfPropertyExistByAddressId(int id);
        void CreateProperty(Property property);
        void UpdateProperty(Property property);
        void DeleteProperty(Property property);
        Task<bool> CheckIfUserOwnsProperty(int loggedUserId, int propertId);
    }
}

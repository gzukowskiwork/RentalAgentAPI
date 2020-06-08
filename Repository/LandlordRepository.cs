using Microsoft.EntityFrameworkCore;
using Contracts;
using Entities;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace Repository
{
    public class LandlordRepository : RepositoryBase<Landlord>, ILandlordRepository
    {

        public LandlordRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public async Task<IEnumerable<Landlord>> GetAllLandlords()
        {
            return await FindAll().OrderBy(l => l.Id).ToListAsync();
        }

        public async Task<Landlord> GetLandlordById(int id)
        {
            return await FindByCondition(l => l.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Landlord> GetLandlordWithProperties(int id)
        {
            return await FindByCondition(l => l.Id.Equals(id)).Include(l => l.Properties).FirstOrDefaultAsync();
        }

        public async Task<Landlord> GetLandlordWithAddress(int id)
        {
            return await FindByCondition(l => l.Id.Equals(id)).Include(l => l.Address).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckIfUserOwnsAddress(int loggedUserId, int landlordId)
        {
            int foundedLandlordId = await FindByCondition(l => l.Id.Equals(landlordId)).Select(x => x.Id).FirstOrDefaultAsync();
            if (foundedLandlordId == loggedUserId)
                return true;
            else
                return false;
        }

        public async Task<Landlord> GetLandlordByEmail(string email)
        {
            return await FindByCondition(l => l.ApplicationUser.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Landlord>> GetLandlordsBySurname(string surname)
        {
            return await FindByCondition(l => l.Surname.Equals(surname)).ToListAsync();
        }

        public async Task<IEnumerable<Landlord>> GetLandlordsByCity(string city)
        {
            return await FindByCondition(l => l.Address.City.Equals(city)).ToListAsync();
        }

        public async Task<IEnumerable<Landlord>> GetLandlordsByStreet(string street)
        {
            return await FindByCondition(l => l.Address.Street.Equals(street))
                .Include(p => p.Properties)
                    .ThenInclude(p => p.Address)
                .Include(l => l.Address).ToListAsync();
        }

        public async Task<IEnumerable<Landlord>> GetLandlordsByZipCode(string zipCode)
        {
            return await FindByCondition(l => l.Address.PostalCode.Equals(zipCode)).ToListAsync();
        }

        public async Task<IEnumerable<Landlord>> GetLandlordsByCountry(string country)
        {
            return await FindByCondition(l => l.Address.Country.Equals(country)).ToListAsync();
        }

        public async Task<IEnumerable<Landlord>> GetLandlordByCompanyName(string company)
        {
            return await FindByCondition(l => l.CompanyName.Equals(company)).ToListAsync();
        }

        public async Task<IEnumerable<Landlord>> GetLandlordsByNip(string nip)
        {
            return await FindByCondition(l => l.NIP.Equals(nip)).ToListAsync();
        }

        public async Task<IEnumerable<Landlord>> GetLandlordsByPesel(string pesel)
        {
            return await FindByCondition(l => l.PESEL.Equals(pesel)).ToListAsync();
        }

        public async Task<IEnumerable<Landlord>> GetLandlordByRegon(string regon)
        {
            return await FindByCondition(l => l.REGON.Equals(regon)).ToListAsync();
        }

        public async Task<Landlord> GetAllRelatedEntitiesByLandlordId(int id)
        {
            return await FindByCondition(l => l.Id.Equals(id))
                .Include(l => l.Properties)
                .Include(l => l.Rents)
                .Include(l => l.Address)
                .FirstOrDefaultAsync();
        }

        public async Task<Boolean> CheckIfLandlordExistByLandlordId(int id)
        {
            return await FindByCondition(l => l.Id.Equals(id)).AnyAsync();
        }

        public async Task<Boolean> CheckIfLandlordWithEmailExist(string email)
        {
            return await FindByCondition(l => l.ApplicationUser.Email.Equals(email)).AnyAsync();
        }

        public async Task<Boolean> CheckIfLandlordExistByAddressId(int id)
        {
            return await FindByCondition(l => l.AddressId.Equals(id)).AnyAsync();
        }

        public async Task<Landlord> GetLandlordByAspNetUserId(int id)
        {
            return await FindByCondition(l => l.AspNetUsersId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckIfLandlordWithAspNetUserIdExist(int id)
        {
            return await FindByCondition(l => l.AspNetUsersId.Equals(id)).AnyAsync();
        }

        public void CreateLandlord(Landlord landlord)
        {
            Create(landlord);
        }

        public void UpdateLandlord(Landlord landlord)
        {
            Update(landlord);
        }

        public void DeleteLandlord(Landlord landlord)
        {
            Delete(landlord);
        }

        public async Task<int> FindLandlordIdByAddressId(int addressId)
        {
            return await FindByCondition(l => l.AddressId.Equals(addressId)).Select(x => x.Id).FirstOrDefaultAsync();
        }




    }
}

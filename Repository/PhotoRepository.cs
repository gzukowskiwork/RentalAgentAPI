using Microsoft.EntityFrameworkCore;
using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.DataTransferObjects.Photo;

namespace Repository
{
    public class PhotoRepository: RepositoryBase<Photo>, IPhotoRepository
    {
        public PhotoRepository(RepositoryContext fakeRentContext): base(fakeRentContext)
        {

        }

        public async Task<Photo> GetPhotoById(int id)
        {
            return await FindByCondition(p => p.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Photo> GetPhotoByStateId(int id)
        {
            return await FindByCondition(p => p.StateId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<byte[]> GetPhotoByIdOneColumn(int id, string photoColumnName)
        {
            return photoColumnName switch
            {
                "coldwater" => await FindByCondition(p => p.Id.Equals(id)).Select(p => p.ColdWaterPhoto).FirstOrDefaultAsync(),
                "hotwater" => await FindByCondition(p => p.Id.Equals(id)).Select(p => p.HotWaterPhoto).FirstOrDefaultAsync(),
                "gas" => await FindByCondition(p => p.Id.Equals(id)).Select(p => p.GasPhoto).FirstOrDefaultAsync(),
                "energy" => await FindByCondition(p => p.Id.Equals(id)).Select(p => p.EnergyPhoto).FirstOrDefaultAsync(),
                "heat" => await FindByCondition(p => p.Id.Equals(id)).Select(p => p.HeatPhoto).FirstOrDefaultAsync(),
                _ => new byte[0],
            };
        }

        public void CreatePhoto(Photo photo)
        {
            Create(photo);
        }

        public void UpdatePhoto(Photo photo)
        {
            Update(photo);
        }

        public void DeletePhoto(Photo photo)
        {
            Delete(photo);
        }
    }
}

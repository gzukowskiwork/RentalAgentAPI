using Entities.DataTransferObjects.Photo;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPhotoRepository: IRepositoryBase<Photo>
    {
        Task<Photo> GetPhotoById(int id);
        Task<Photo> GetPhotoByStateId(int id);
        Task<byte[]> GetPhotoByIdOneColumn(int id, string photoColumnName);
        public void CreatePhoto(Photo photo);
        public void UpdatePhoto(Photo photo);
        public void DeletePhoto(Photo photo);
    }
}

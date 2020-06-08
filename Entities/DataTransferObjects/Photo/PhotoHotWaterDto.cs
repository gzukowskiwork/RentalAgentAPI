using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Photo
{
    public class PhotoHotWaterDto
    {
        public int Id { get; set; }

        public Byte[] HotWaterPhoto { get; set; }

        public DateTime? HotWaterExif { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int StateId { get; set; }
    }
}

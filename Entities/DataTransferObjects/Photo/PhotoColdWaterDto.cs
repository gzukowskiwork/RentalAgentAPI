using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Photo
{
    public class PhotoColdWaterDto
    {
        public int Id { get; set; }

        public Byte[] ColdWaterPhoto { get; set; }

        public DateTime? ColdWaterExif { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int StateId { get; set; }
    }
}

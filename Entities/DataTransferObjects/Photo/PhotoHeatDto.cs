using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Photo
{
    public class PhotoHeatDto
    {
        public int Id { get; set; }

        public Byte[] HeatPhoto { get; set; }

        public DateTime? HeatExif { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int StateId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Photo
{
    public class PhotoEnergyDto
    {
        public int Id { get; set; }

        public Byte[] EnergyPhoto { get; set; }

        public DateTime? EnergyExif { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int StateId { get; set; }
    }
}

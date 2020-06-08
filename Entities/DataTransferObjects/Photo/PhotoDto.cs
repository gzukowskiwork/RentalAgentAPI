using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Photo
{
    public class PhotoDto
    {
        public int Id { get; set; }

        public Byte[] ColdWaterPhoto { get; set; }

        public Byte[] HotWaterPhoto { get; set; }

        public Byte[] GasPhoto { get; set; }

        public Byte[] EnergyPhoto { get; set; }

        public Byte[] HeatPhoto { get; set; }

        public DateTime? ColdWaterExif { get; set; }

        public DateTime? HotWaterExif { get; set; }

        public DateTime? GasExif { get; set; }

        public DateTime? EnergyExif { get; set; }

        public DateTime? HeatExif { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int StateId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects.Photo
{
    public class PhotoForCreationDto
    {
        [Required(ErrorMessage = "Cold water photo is required")]
        public Byte[] ColdWaterPhoto { get; set; }

        public Byte[] HotWaterPhoto { get; set; }

        public Byte[] GasPhoto { get; set; }

        [Required(ErrorMessage = "Energy photo is required")]
        public Byte[] EnergyPhoto { get; set; }

        public Byte[] HeatPhoto { get; set; }

        [Required(ErrorMessage = "Cold water exif is required")]
        public DateTime? ColdWaterExif { get; set; }

        public DateTime? HotWaterExif { get; set; }

        public DateTime? GasExif { get; set; }

        [Required(ErrorMessage = "Energy exif is required")]
        public DateTime? EnergyExif { get; set; }

        public DateTime? HeatExif { get; set; }

        [Required(ErrorMessage = "StateId is required")]
        public int StateId { get; set; }
    }
}

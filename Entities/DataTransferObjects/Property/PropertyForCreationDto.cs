using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects.Property
{
    public class PropertyForCreationDto
    {
        [Required(ErrorMessage = "FlatLabel is required.")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Label must be <2,255> char long")]
        public string FlatLabel { get; set; }

        [DataType(DataType.Upload)]
        public Byte[] Thumbnail { get; set; }

        [Required(ErrorMessage = "RoomCount is required.")]
        public short RoomCount { get; set; }

        [Required(ErrorMessage = "FlatSize is required.")]
        public decimal FlatSize { get; set; }

        [Required(ErrorMessage = "HasGas is required.")]
        [DefaultValue(false)]
        public bool HasGas { get; set; } = false;

        [Required(ErrorMessage = "HasHW is required.")]
        [DefaultValue(true)]
        public bool HasHW { get; set; } = false;

        [Required(ErrorMessage = "HasHeat is required.")]
        [DefaultValue(true)]
        public bool HasHeat { get; set; } = false;

        [DefaultValue(true)]
        public bool DisplayOnWeb { get; set; } = true;

        [StringLength(255, MinimumLength = 0, ErrorMessage = "Label must be <0,255> char long")]
        public string LandlordComment { get; set; }

        [Required(ErrorMessage = "LandlordId is required.")]
        public int LandlordId { get; set; }

        [Required(ErrorMessage = "AddressId is required.")]
        [DefaultValue(1)]
        public int AddressId { get; set; } = 1;

    }
}

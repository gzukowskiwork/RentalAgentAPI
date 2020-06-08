using Entities.DataTransferObjects.Address;
using Entities.DataTransferObjects.Rate;
using Entities.DataTransferObjects.Tenant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Property
{
    public class PropertyWithAddressAndRateDto
    {
        public int Id { get; set; }
        public string FlatLabel { get; set; }
        public Byte[] Thumbnail { get; set; }
        public short RoomCount { get; set; }
        public decimal FlatSize { get; set; }
        public bool HasGas { get; set; }
        public bool HasHW { get; set; }
        public bool HasHeat { get; set; }
        public bool DisplayOnWeb { get; set; }
        public string LandlordComment { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int LandlordId { get; set; }
        public int AddressId { get; set; }

        public AddressDto Address { get; set; }
        public RateDto Rate { get; set; }

    }
}

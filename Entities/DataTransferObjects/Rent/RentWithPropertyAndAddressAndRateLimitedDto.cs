﻿using Entities.DataTransferObjects.Property;
using Entities.DataTransferObjects.Rate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Rent
{
    public class RentWithPropertyAndAddressAndRateLimitedDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int TenantId { get; set; }
        public int LandlordId { get; set; }
        public string RentPurpose { get; set; }
        public DateTime? StartRent { get; set; }
        public DateTime? EndRent { get; set; }
        public short TenantCount { get; set; }
        public decimal RentDeposit { get; set; }
        public short PayDayDelay { get; set; }
        public short SendStateDay { get; set; }
        public bool DisplayOnWeb { get; set; }
        public string LandlordComment { get; set; }
        public bool PhotoRequired { get; set; }

        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public PropertyWithRateAndAddressDto Property { get; set; }
    }
}

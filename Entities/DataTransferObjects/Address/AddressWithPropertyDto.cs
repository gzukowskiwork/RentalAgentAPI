﻿using System;
using Entities.DataTransferObjects.Property;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Address
{
    public class AddressWithPropertyDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }
        public string PostalCode { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public PropertyDto Property { get; set; }
    }
}

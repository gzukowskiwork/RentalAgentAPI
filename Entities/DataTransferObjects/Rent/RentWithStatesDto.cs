using Entities.DataTransferObjects.State;
using System;
using System.Collections.Generic;


namespace Entities.DataTransferObjects.Rent
{
    public class RentWithStatesDto
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
        public Byte[] Contract { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public IEnumerable<StateDto> States { get; set; }
    }
}

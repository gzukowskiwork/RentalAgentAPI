using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Tenant
{
    public class TenantDto
    {
        public int Id { get; set; }
        public string NIP { get; set; }
        public string PESEL { get; set; }
        public string REGON { get; set; }
        public bool IsCompany { get; set; }
        public string CompanyName { get; set; }
        public string PhonePrefix { get; set; }
        public string PhoneNumber { get; set; }
        public string BankAccount { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool DisplayOnWeb { get; set; }
        public string LandlordComment { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int AddressId { get; set; }
        public int AspNetUsersId { get; set; }
    }
}

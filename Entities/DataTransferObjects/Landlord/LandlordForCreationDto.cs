using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects.Landlord
{
    public class LandlordForCreationDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Name must be <3,45> char long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Surname must be <3,45> char long")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "IsCompany is required.")]
        public bool IsCompany { get; set; }

        [Required(ErrorMessage = "IsVATPayer is required.")]
        public bool IsVATPayer { get; set; }

        [StringLength(45, MinimumLength = 3, ErrorMessage = "Company Name must be <3,45> char long")]
        public string CompanyName { get; set; }

        [DataType(DataType.Upload)]
        public Byte[] Logo { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "NIP number must be exactly 10 digit long")]
        public string NIP { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL number must be exactly 11 digit long")]
        public string PESEL { get; set; }

        [StringLength(9, MinimumLength = 9, ErrorMessage = "REGON must be exactly 9 digit long")]
        public string REGON { get; set; }

        [Required(ErrorMessage = "PhonePrefix is required.")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Prefix must be exactly 4 digit long")]
        public string PhonePrefix { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "Phone Number must be <9-10> digit long")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "BankAccount is required.")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(26, MinimumLength = 26, ErrorMessage = "Bank Account must be exactly 26 digit long")]
        public string BankAccount { get; set; }

        [Required(ErrorMessage = "AddressId is required.")]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "AspNetUsersId is required.")]
        public int AspNetUsersId { get; set; }
    }
}

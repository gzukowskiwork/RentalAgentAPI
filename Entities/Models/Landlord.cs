using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("Landlord")]
    public class Landlord
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Name must be <3,45> char long")]
        public string Name { get; set; }

        [Column("Surname")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Surname must be <3,45> char long")]
        public string Surname { get; set; }

        [Required]
        [Column("IsCompany")]
        public bool IsCompany { get; set; }

        [Required]
        [Column("IsVATPayer")]
        public bool IsVATPayer { get; set; }

        [Column("CompanyName")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Company Name must be <3,45> char long")]
        public string CompanyName { get; set; }

        [Column("Logo")]
        [DataType(DataType.Upload)]
        public Byte[] Logo { get; set; }

        [Column("NIP")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "NIP number must be exactly 10 digit long")]
        public string NIP { get; set; }

        [Column("PESEL")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL number must be exactly 11 digit long")]
        public string PESEL { get; set; }

        [Column("REGON")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "REGON must be exactly 9 digit long")]
        public string REGON { get; set; }

        [Column("PhonePrefix")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Prefix must be exactly 4 digit long")]
        public string PhonePrefix { get; set; }

        [Column("PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "Phone Number must be <9-10> digit long")]
        public string PhoneNumber { get; set; }

        [Column("BankAccount")]
        [StringLength(26, MinimumLength = 26, ErrorMessage = "Bank Account must be excatly 26 digit long")]
        public string BankAccount { get; set; }

        [Timestamp]
        [Column("CreateTime")]
        public DateTime? CreateTime { get; set; }

        [Timestamp]
        [Column("UpdateTime")]
        public DateTime? UpdateTime { get; set; }

        //one to one relation with address
        [ForeignKey(nameof(Address))]
        [Column("AddressId")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        //one to one relation with AspNetUser
        [ForeignKey(nameof(ApplicationUser))]
        [Column("AspNetUsersId")]
        public int AspNetUsersId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Property> Properties { get; set; }

        public ICollection<Rent> Rents { get; set; }
    }
}

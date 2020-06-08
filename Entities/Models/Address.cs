using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Models
{
    //Bolow decorator maps our class (that can be named anyhow) to specific DB Table name - this Case must be 'Address'.
    [Table("Address")] 
    public class Address
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("Country")]
        [DataType(DataType.Text)]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Country must be <3,45> char long")]
        public string Country { get; set; }

        [Required]
        [Column("City")]
        [DataType(DataType.Text)]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "City must be <3,45> char long")]
        public string City { get; set; }

        [Required]
        [Column("Street")]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Street must be <3,100> char long")]
        public string Street { get; set; }

        [Required]
        [Column("BuildingNumber")]
        [DataType(DataType.Text)]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Building Number must be <1,10> digit long")]
        public string BuildingNumber { get; set; }

        [Column("FlatNumber")]
        [DataType(DataType.Text)]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Flat Number must be <1,10> digit long")]
        public string FlatNumber { get; set; }

        [Required]
        [Column("PostalCode")]
        [DataType(DataType.Text)]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "Postal Code must be <1,16> digit long")]
        public string PostalCode { get; set; }

        [Timestamp]
        [Column("CreateTime")]
        public DateTime? CreateTime { get; set; }

        [Timestamp]
        [Column("UpdateTime")]
        public DateTime? UpdateTime { get; set; }


        public Landlord Landlord { get; set; }

        public Property Property { get; set; }

        public Tenant Tenant { get; set; }
    }
}

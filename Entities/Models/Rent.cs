using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("Rent")]
    public class Rent
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey(nameof(Property))]
        [Column("PropertyId")]
        public int PropertyId { get; set; }
        public Property Property { get; set; }

        [ForeignKey(nameof(Tenant))]
        [Column("TenantId")]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [ForeignKey(nameof(Landlord))]
        [Column("LandlordId")]
        public int LandlordId { get; set; }
        public Landlord Landlord { get; set; }

        [Required]
        [Column("RentPurpose")]
        [DataType(DataType.Text)]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "RentPurpose must be 4-10 char long (live, work, hotel)")]
        public string RentPurpose { get; set; }

        [Required]
        [Column("StartRent")]
        [DataType(DataType.Date)]
        public DateTime? StartRent { get; set; }

        [Required]
        [Column("EndRent")]
        [DataType(DataType.Date)]
        public DateTime? EndRent { get; set; }

        [Required]
        [Column("TenantCount")]
        public short TenantCount { get; set; }

        [Required]
        [Column("RentDeposit")]
        public decimal RentDeposit { get; set; }

        [Required]
        [Column("PayDayDelay")]
        public short PayDayDelay { get; set; }

        [Required]
        [Column("SendStateDay")]
        public short SendStateDay { get; set; }

        [Column("DisplayOnWeb")]
        public bool DisplayOnWeb { get; set; }

        [Column("LandlordComment")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "Comment must be <0,255> char long")]
        public string LandlordComment { get; set; }

        [Column("PhotoRequired")]
        public bool PhotoRequired { get; set; }

        [Column("Contract")]
        [DataType(DataType.Upload)]
        public Byte[] Contract { get; set; }

        [Timestamp]
        [Column("CreateTime")]
        public DateTime? CreateTime { get; set; }

        [Timestamp]
        [Column("UpdateTime")]
        public DateTime? UpdateTime { get; set; }


        public ICollection<Invoice> Invoices { get; set; }

        public ICollection<State> States { get; set; }

    }
}

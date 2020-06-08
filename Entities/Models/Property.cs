using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities.Models
{
    [Table("Property")]
    public class Property
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("FlatLabel")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Label must be <2,255> char long")]
        public string FlatLabel { get; set; }

        [Column("Thumbnail")]
        [DataType(DataType.Upload)]
        public Byte[] Thumbnail { get; set; }

        [Required]
        [Column("RoomCount")]
        public short RoomCount { get; set; }

        [Required]
        [Column("FlatSize")]
        public decimal FlatSize { get; set; }

        [Required]
        [Column("HasGas")]
        public bool HasGas { get; set; }

        [Required]
        [Column("HasHW")]
        public bool HasHW { get; set; }

        [Required]
        [Column("HasHeat")]
        public bool HasHeat { get; set; }

        [Required]
        [Column("DisplayOnWeb")]
        public bool DisplayOnWeb { get; set; }

        [Column("LandlordComment")]
        public string LandlordComment { get; set; }

        [Timestamp]
        [Column("CreateTime")]
        public DateTime? CreateTime { get; set; }

        [Timestamp]
        [Column("UpdateTime")]
        public DateTime? UpdateTime { get; set; }

        [ForeignKey(nameof(Landlord))]
        [Column("LandlordId")]
        public int LandlordId { get; set; }
        public Landlord Landlord { get; set; }

        [ForeignKey(nameof(Address))]
        [Column("AddressId")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public Rate Rate { get; set; }

        //public ICollection<Tenant> Tenants { get; set; }
        public ICollection<Rent> Rents { get; set; }

    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Photo")]
    public class Photo
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("ColdWaterPhoto")]
        [DataType(DataType.Upload)]
        public Byte[] ColdWaterPhoto { get; set; }

        [Column("HotWaterPhoto")]
        [DataType(DataType.Upload)]
        public Byte[] HotWaterPhoto { get; set; }

        [Column("GasPhoto")]
        [DataType(DataType.Upload)]
        public Byte[] GasPhoto { get; set; }

        [Column("EnergyPhoto")]
        [DataType(DataType.Upload)]
        public Byte[] EnergyPhoto { get; set; }

        [Column("HeatPhoto")]
        [DataType(DataType.Upload)]
        public Byte[] HeatPhoto { get; set; }

        [Column("ColdWaterExif")]
        [DataType(DataType.Date)]
        public DateTime? ColdWaterExif { get; set; }

        [Column("HotWaterExif")]
        [DataType(DataType.Date)]
        public DateTime? HotWaterExif { get; set; }

        [Column("GasExif")]
        [DataType(DataType.Date)]
        public DateTime? GasExif { get; set; }

        [Column("EnergyExif")]
        [DataType(DataType.Date)]
        public DateTime? EnergyExif { get; set; }

        [Column("HeatExif")]
        [DataType(DataType.Date)]
        public DateTime? HeatExif { get; set; }

        [Column("CreateTime")]
        [Timestamp]
        public DateTime? CreateTime { get; set; }

        [Column("UpdateTime")]
        [Timestamp]
        public DateTime? UpdateTime { get; set; }

        [ForeignKey(nameof(State))]
        [Column("StateId")]
        public int StateId { get; set; }
        public State State { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("State")]
    public class State
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("ColdWaterState")]
        public decimal ColdWaterState { get; set; }

        [Column("HotWaterState")]
        public decimal? HotWaterState { get; set; }

        [Column("GasState")]
        public decimal? GasState { get; set; }

        [Column("EnergyState")]
        public decimal EnergyState { get; set; }

        [Column("HeatState")]
        public decimal? HeatState { get; set; }

        [Timestamp]
        [Column("CreateTime")]
        public DateTime? CreateTime { get; set; }

        [Timestamp]
        [Column("UpdateTime")]
        public DateTime? UpdateTime { get; set; }

        [Column("IsInitial")]
        public bool IsInitial { get; set; }

        [Column("IsConfirmed")]
        public bool IsConfirmed { get; set; }

        [ForeignKey(nameof(Rent))]
        [Column("RentId")]
        public int RentId { get; set; }
        public Rent Rent { get; set; }

        public Invoice Invoice { get; set; }

        public Photo Photo { get; set; }
    }
}

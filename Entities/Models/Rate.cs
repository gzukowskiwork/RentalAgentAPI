using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Rate")]
    public class Rate
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("LandlordRent")]
        public decimal LandlordRent { get; set; }

        [Column("HousingRent")]
        public decimal HousingRent { get; set; }

        [Column("ColdWaterPrice")]
        public decimal ColdWaterPrice { get; set; }

        [Column("HotWaterPrice")]
        public decimal? HotWaterPrice { get; set; }

        [Column("GasPrice")]
        public decimal? GasPrice { get; set; }

        [Column("EnergyPrice")]
        public decimal EnergyPrice { get; set; }

        [Column("HeatPrice")]
        public decimal? HeatPrice { get; set; }

        [Column("GasSubscription")]
        public decimal? GasSubscription { get; set; }

        [Column("EnergySubscription")]
        public decimal? EnergySubscription { get; set; }

        [Column("HeatSubscription")]
        public decimal? HeatSubscription { get; set; }

        [Column("LandlordRentVAT")]
        public decimal? LandlordRentVAT { get; set; }

        [Column("HousingRentVAT")]
        public decimal? HousingRentVAT { get; set; }

        [Column("WaterVAT")]
        public decimal? WaterVAT { get; set; }

        [Column("GasVAT")]
        public decimal? GasVAT { get; set; }

        [Column("EnergyVAT")]
        public decimal? EnergyVAT { get; set; }

        [Column("HeatVAT")]
        public decimal? HeatVAT { get; set; }

        [Timestamp]
        [Column("CreateTime")]
        public DateTime? CreateTime { get; set; }

        [Timestamp]
        [Column("UpdateTime")]
        public DateTime? UpdateTime { get; set; }

        [ForeignKey(nameof(Property))]
        [Column("PropertyId")]
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}

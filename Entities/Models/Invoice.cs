using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("Invoice")]
    public class Invoice
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
        public decimal GasSubscription { get; set; }

        [Column("EnergySubscription")]
        public decimal EnergySubscription { get; set; }

        [Column("HeatSubscription")]
        public decimal? HeatSubscription { get; set; }

        [Column("ColdWaterConsumption")]
        public decimal ColdWaterConsumption { get; set; }

        [Column("HotWaterConsumption")]
        public decimal? HotWaterConsumption { get; set; }

        [Column("GasConsumption")]
        public decimal? GasConsumption { get; set; }

        [Column("EnergyConsumption")]
        public decimal EnergyConsumption { get; set; }

        [Column("HeatConsumption")]
        public decimal? HeatConsumption { get; set; }

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

        [Column("LandlordRentVAT")]
        public decimal LandlordRentVAT { get; set; }

        [Column("HousingRentVAT")]
        public decimal HousingRentVAT { get; set; }

        [Column("WaterVAT")]
        public decimal WaterVAT { get; set; }

        [Column("GasVAT")]
        public decimal? GasVAT { get; set; }

        [Column("EnergyVAT")]
        public decimal EnergyVAT { get; set; }

        [Column("HeatVAT")]
        public decimal? HeatVAT { get; set; }

        [Column("LandlordComment")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "Comment must be <0,255> char long")]
        public string LandlordComment { get; set; }

        [Column("Invoice")]
        [DataType(DataType.Upload)]
        public Byte[] InvoiceDocument { get; set; }

        [Column("FileName")]
        [StringLength(255, MinimumLength = 0, ErrorMessage = "FileName must be <0,255> char long")]
        public string FileName { get; set; }

        [Timestamp]
        [Column("CreateTime")]
        public DateTime? CreateTime { get; set; }

        [Timestamp]
        [Column("UpdateTime")]
        public DateTime? UpdateTime { get; set; }

        [Column("IsDistributed")]
        public bool IsDistributed { get; set; }

        [ForeignKey(nameof(Rent))]
        [Column("RentId")]
        public int RentId { get; set; }
        public Rent Rent { get; set; }

        [ForeignKey(nameof(State))]
        [Column("StateId")]
        public int StateId { get; set; }
        public State State { get; set; }


    }
}
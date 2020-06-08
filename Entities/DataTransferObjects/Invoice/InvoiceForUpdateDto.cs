using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects.Invoice
{
    public class InvoiceForUpdateDto
    {
        [Required(ErrorMessage = "LandlordRent is required.")]
        public decimal LandlordRent { get; set; }

        [Required(ErrorMessage = "HousingRent is required.")]
        public decimal HousingRent { get; set; }

        [Required(ErrorMessage = "ColdWaterPrice is required.")]
        public decimal ColdWaterPrice { get; set; }

        public decimal? HotWaterPrice { get; set; }

        public decimal? GasPrice { get; set; } = 0;

        [Required(ErrorMessage = "EnergyPrice is required.")]
        public decimal EnergyPrice { get; set; }

        public decimal? HeatPrice { get; set; } = 0;

        public decimal? GasSubscription { get; set; } = 0;

        [Required(ErrorMessage = "EnergySubscription is required.")]
        public decimal EnergySubscription { get; set; }

        public decimal? HeatSubscription { get; set; } = 0;

        [Required(ErrorMessage = "ColdWaterConsumption is required.")]
        public decimal ColdWaterConsumption { get; set; }

        public decimal? HotWaterConsumption { get; set; } = 0;

        public decimal? GasConsumption { get; set; } = 0;

        [Required(ErrorMessage = "EnergyConsumption is required.")]
        public decimal EnergyConsumption { get; set; }

        public decimal? HeatConsumption { get; set; } = 0;

        [Required(ErrorMessage = "ColdWaterState is required.")]
        public decimal ColdWaterState { get; set; }

        public decimal? HotWaterState { get; set; }

        public decimal? GasState { get; set; }

        [Required(ErrorMessage = "EnergyState is required.")]
        public decimal EnergyState { get; set; }

        public decimal? HeatState { get; set; }

        [Required(ErrorMessage = "LandlordRentVAT is required.")]
        public decimal LandlordRentVAT { get; set; }

        [Required(ErrorMessage = "HousingRentVAT is required.")]
        public decimal HousingRentVAT { get; set; }

        [Required(ErrorMessage = "WaterVAT is required.")]
        public decimal WaterVAT { get; set; }

        public decimal? GasVAT { get; set; }

        [Required(ErrorMessage = "EnergyVAT is required.")]
        public decimal EnergyVAT { get; set; }

        public decimal? HeatVAT { get; set; }

        public Byte[] InvoiceDocument { get; set; }

        [StringLength(255, MinimumLength = 0, ErrorMessage = "FileName must be <0,255> char long")]
        public string FileName { get; set; }

        [StringLength(255, MinimumLength = 0, ErrorMessage = "LandlordComment must be <0,255> char long")]
        public string LandlordComment { get; set; }

        [Required(ErrorMessage = "IsDistributed is required.")]
        public bool IsDistributed { get; set; }

        [Required(ErrorMessage = "RentId is required.")]
        public int RentId { get; set; }

        [Required(ErrorMessage = "StateId is required.")]
        public int StateId { get; set; }
    }
}

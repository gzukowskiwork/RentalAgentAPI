using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects.Rate
{
    public class RateForUpdateDto
    {
        [Required(ErrorMessage = "LandlordRent is required.")]
        public decimal LandlordRent { get; set; }

        [Required(ErrorMessage = "HousingRent is required.")]
        public decimal HousingRent { get; set; }

        [Required(ErrorMessage = "ColdWaterPrice is required.")]
        public decimal ColdWaterPrice { get; set; }

        public decimal? HotWaterPrice { get; set; }

        public decimal? GasPrice { get; set; }

        [Required(ErrorMessage = "EnergyPrice is required.")]
        public decimal EnergyPrice { get; set; }

        public decimal? HeatPrice { get; set; }

        public decimal? GasSubscription { get; set; }

        [Required(ErrorMessage = "EnergySubscription is required.")]
        public decimal? EnergySubscription { get; set; }

        public decimal? HeatSubscription { get; set; }

        [Required(ErrorMessage = "LandlordRentVAT is required.")]
        public decimal? LandlordRentVAT { get; set; }

        [Required(ErrorMessage = "HousingRentVAT is required.")]
        public decimal? HousingRentVAT { get; set; }

        [Required(ErrorMessage = "WaterVAT is required.")]
        public decimal? WaterVAT { get; set; }

        public decimal? GasVAT { get; set; }

        [Required(ErrorMessage = "EnergyVAT is required.")]
        public decimal? EnergyVAT { get; set; }

        public decimal? HeatVAT { get; set; }

        [Required(ErrorMessage = "PropertyId is required.")]
        public int PropertyId { get; set; }
    }
}

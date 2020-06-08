using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Rate
{
    public class RateDto
    {
        public int Id { get; set; }
        public decimal LandlordRent { get; set; }
        public decimal HousingRent { get; set; }
        public decimal ColdWaterPrice { get; set; }
        public decimal? HotWaterPrice { get; set; }
        public decimal? GasPrice { get; set; }
        public decimal EnergyPrice { get; set; }
        public decimal? HeatPrice { get; set; }
        public decimal? GasSubscription { get; set; }
        public decimal? EnergySubscription { get; set; }
        public decimal? HeatSubscription { get; set; }
        public decimal? LandlordRentVAT { get; set; }
        public decimal? HousingRentVAT { get; set; }
        public decimal? WaterVAT { get; set; }
        public decimal? GasVAT { get; set; }
        public decimal? EnergyVAT { get; set; }
        public decimal? HeatVAT { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int PropertyId { get; set; }
    }
}

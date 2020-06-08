using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.Invoice
{
    public class InvoiceDto
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
        public decimal EnergySubscription { get; set; }
        public decimal? HeatSubscription { get; set; }
        public decimal ColdWaterConsumption { get; set; }
        public decimal? HotWaterConsumption { get; set; }
        public decimal? GasConsumption { get; set; }
        public decimal EnergyConsumption { get; set; }
        public decimal? HeatConsumption { get; set; }
        public decimal ColdWaterState { get; set; }
        public decimal? HotWaterState { get; set; }
        public decimal? GasState { get; set; }
        public decimal EnergyState { get; set; }
        public decimal? HeatState { get; set; }
        public decimal LandlordRentVAT { get; set; }
        public decimal HousingRentVAT { get; set; }
        public decimal WaterVAT { get; set; }
        public decimal? GasVAT { get; set; }
        public decimal EnergyVAT { get; set; }
        public decimal? HeatVAT { get; set; }
        public string LandlordComment { get; set; }
        public Byte[] InvoiceDocument { get; set; }
        public string FileName { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsDistributed { get; set; }
        public int RentId { get; set; }
        public int StateId { get; set; }

    }
}

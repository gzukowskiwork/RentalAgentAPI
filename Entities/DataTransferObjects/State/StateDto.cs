using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects.State
{
    public class StateDto
    {
        public int Id { get; set; }
        public decimal ColdWaterState { get; set; }
        public decimal? HotWaterState { get; set; }
        public decimal? GasState { get; set; }
        public decimal EnergyState { get; set; }
        public decimal? HeatState { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsInitial { get; set; }
        public bool? IsConfirmed { get; set; }
        public int RentId { get; set; }
    }
}

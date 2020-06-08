using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects.State
{
    public class StateForCreationDto
    {
        [Required(ErrorMessage = "ColdWaterState is required.")]
        public decimal ColdWaterState { get; set; }

        public decimal? HotWaterState { get; set; }

        public decimal? GasState { get; set; }

        [Required(ErrorMessage = "EnergyState is required.")]
        public decimal EnergyState { get; set; }

        public decimal? HeatState { get; set; }

        public bool IsInitial { get; set; }

        public bool? IsConfirmed { get; set; }

        [Required(ErrorMessage = "RentId is required.")]
        public int RentId { get; set; }
    }
}

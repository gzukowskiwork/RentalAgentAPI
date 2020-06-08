using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects.Rent
{
    public class RentForCreationDto
    {
        [Required(ErrorMessage = "PropertyId is required.")]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "TenantId is required.")]
        public int TenantId { get; set; }

        [Required(ErrorMessage = "LandlordId is required.")]
        public int LandlordId { get; set; }

        [Required(ErrorMessage = "RentPurpose must be <3,45> char long")]
        [DataType(DataType.Text)]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "RentPurpose must be <4,10> char long")]
        public string RentPurpose { get; set; }

        [Required(ErrorMessage = "StartRent is required.")]
        [DataType(DataType.Date)]
        public DateTime? StartRent { get; set; }

        [Required(ErrorMessage = "EndRent is required.")]
        [DataType(DataType.Date)]
        public DateTime? EndRent { get; set; }

        [Required(ErrorMessage = "PayDayDelay is required.")]
        public short TenantCount { get; set; }

        [Required(ErrorMessage = "RentDeposit is required.")]
        public decimal RentDeposit { get; set; }

        [Required(ErrorMessage = "PayDayDelay is required.")]
        public short PayDayDelay { get; set; }

        [Required(ErrorMessage = "SendStateDay is required.")]
        public short SendStateDay { get; set; }

        public bool DisplayOnWeb { get; set; }

        [StringLength(255, MinimumLength = 0, ErrorMessage = "Comment must be <0,255> char long")]
        public string LandlordComment { get; set; }

        public bool PhotoRequired { get; set; }

        [DataType(DataType.Upload)]
        public Byte[] Contract { get; set; }

    }
}

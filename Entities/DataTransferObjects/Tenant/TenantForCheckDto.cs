using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects.Tenant
{
    public class TenantForCheckDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL number must be exactly 11 digit long")]
        public string PESEL { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "NIP number must be exactly 10 digit long")]
        public string NIP { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "Phone Number must be <9,10> digit long")]
        public string PhoneNumber { get; set; }

    }
}

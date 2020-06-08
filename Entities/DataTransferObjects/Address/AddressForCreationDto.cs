using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects.Address
{
    public class AddressForCreationDto
    {
        [Required(ErrorMessage = "Country is required.")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Country must be <3,45> char long")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "City must be <3,45> char long")]
        public string City { get; set; }

        [Required(ErrorMessage = "Street is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Street must be <3,100> char long")]
        public string Street { get; set; }

        [Required(ErrorMessage = "BuildingNumber is required.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Building Number must be <1,10> digit long")]
        public string BuildingNumber { get; set; }


        [StringLength(10, MinimumLength = 1, ErrorMessage = "Flat Number must be <1,10> digit long")]
        public string FlatNumber { get; set; }

        [Required(ErrorMessage = "PostalCode is required.")]
        [StringLength(16, MinimumLength = 1, ErrorMessage = "Postal Code must be <1,16> digit long")]
        public string PostalCode { get; set; }
    }
}

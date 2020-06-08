using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Identity.Requests
{
    public class SoftDeleteRequest
    {
        [Required]
        public string email { get; set; }
    }
}

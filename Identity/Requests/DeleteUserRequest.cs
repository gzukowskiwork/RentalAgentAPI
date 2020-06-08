using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Identity.Requests
{
    public class DeleteUserRequest
    {
        [Required]
        public int aspUserId { get; set; }
    }
}

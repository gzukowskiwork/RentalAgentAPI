
using System.ComponentModel.DataAnnotations;

namespace Identity.Requests
{
    public class UserRegistrationRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}

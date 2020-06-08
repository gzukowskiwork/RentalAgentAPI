using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public bool FirstLogin { get; set; }
        public Landlord Landlord { get; set; }
        public Tenant Tenant { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Response
{
    public class ResetPasswordResult: MainResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        
    }
}

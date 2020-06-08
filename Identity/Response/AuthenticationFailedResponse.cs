using System.Collections.Generic;

namespace Identity.Response
{
    public class AuthenticationFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}

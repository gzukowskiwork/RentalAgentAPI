using System.Collections.Generic;

namespace Identity.Response
{
    public class MainResponse: SimpleResponse
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}

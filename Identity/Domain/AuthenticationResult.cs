
namespace Identity.Domain
{
    public class AuthenticationResult: Response.MainResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }
    }
}

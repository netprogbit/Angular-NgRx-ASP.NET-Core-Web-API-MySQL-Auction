using System.Collections.Generic;

namespace Server.DTOs
{
    public class TokenDTO
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}

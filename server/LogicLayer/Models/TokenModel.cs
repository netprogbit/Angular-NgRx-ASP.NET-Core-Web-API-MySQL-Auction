using System.Collections.Generic;

namespace LogicLayer.Models
{
    public class TokenModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}

using System;

namespace LogicLayer.Models
{
    public class RefreshTokenModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}

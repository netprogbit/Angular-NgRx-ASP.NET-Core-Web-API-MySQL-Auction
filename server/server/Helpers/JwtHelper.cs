using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Server.Helpers
{
    public static class JwtHelper
    {
        public static readonly string Issuer = Startup.StaticConfig["AppSettings:UrlOrigin"]; // Server URL
        public static readonly string Audience = Startup.StaticConfig["AppSettings:UrlClient"]; // Client URL

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.StaticConfig["AppSettings:Secret"]));
        }
    }
}

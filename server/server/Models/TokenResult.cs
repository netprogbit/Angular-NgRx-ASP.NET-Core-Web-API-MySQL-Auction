namespace Server.Models
{
    public class TokenResult
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        
        public TokenResult(long userId, string token, string role)
        {
            UserId = userId;
            Token = token;
            Role = role;
        }
    }
}

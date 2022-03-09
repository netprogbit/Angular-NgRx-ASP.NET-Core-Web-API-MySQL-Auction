using LogicLayer.Models;

namespace LogicLayer.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UserModel userModel);
        RefreshTokenModel GenerateRefreshToken(string userId);
    }
}

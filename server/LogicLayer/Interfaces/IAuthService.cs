using LogicLayer.Models;
using System.Threading.Tasks;

namespace LogicLayer.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(UserModel userModel);
        Task<TokenModel> AuthenticateAsync(UserModel userModel);
        Task<TokenModel> RefreshTokenAsync(TokenModel tokenModel);
        Task RevokeToken(TokenModel tokenModel);
    }
}

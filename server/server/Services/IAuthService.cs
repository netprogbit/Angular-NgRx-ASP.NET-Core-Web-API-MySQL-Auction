using DataLayer.Entities;
using Server.Models;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IAuthService
    {
        Task<TokenResult> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(User user);
    }
}
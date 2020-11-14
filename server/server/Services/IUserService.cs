using DataLayer.Entities;
using Server.Models;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IUserService
    {
        Task DeleteUserAsync(long id);
        Task<PaginationResult<UserResult>> GetUsersAsync(string searchTerm, int pageIndex, int pageSize);
        Task UpdateUserAsync(User user);
    }
}
using LogicLayer.Models;
using System.Threading.Tasks;

namespace LogicLayer.Interfaces
{
    public interface IUserService
    {
        Task<PaginationModel<UserModel>> GetUserPageAsync(string searchTerm, int pageIndex, int pageSize);
        Task UpdateUserAsync(UserModel userModel);
        Task DeleteUserByIdAsync(string id);
    }
}

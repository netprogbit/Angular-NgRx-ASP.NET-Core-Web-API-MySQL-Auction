using LogicLayer.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LogicLayer.InterfacesOut.Account
{
    public interface IUserRepository : IAccountGenericRepository<UserModel>
    {
        Task<PaginationModel<UserModel>> GetUserPageAsync(string searchTerm, int pageIndex, int pageSize);
        Task<UserModel> GetUserWithRolesAsync(Expression<Func<UserModel, bool>> predicate);
    }
}

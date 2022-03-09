using AutoMapper;
using DataLayer.Contexts;
using DataLayer.Entities.Account;
using LogicLayer.InterfacesOut.Account;
using LogicLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Account
{
    public class UserRepository : AccountGenericRepository<UserModel, User>, IUserRepository
    {
        private readonly AccountDbContext _userDbContext;

        public UserRepository(IMapper mapper, AccountDbContext userDbContext)
            : base(mapper, userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<PaginationModel<UserModel>> GetUserPageAsync(string searchTerm, int pageIndex, int pageSize)
        {
            var queryUsers = _userDbContext.Users.AsNoTracking().Where(u => u.UserName.Contains(searchTerm));
            int count = await queryUsers.CountAsync();
            var querySelectedUsers = queryUsers.OrderBy(u => u.UserName).Skip(pageIndex * pageSize).Take(pageSize);
            var selectedUsers = await querySelectedUsers.ToListAsync();
            var productModels = _mapper.Map<IEnumerable<UserModel>>(selectedUsers);
            return new PaginationModel<UserModel> { Items = productModels, Length = count };
        }

        public async Task<UserModel> GetUserWithRolesAsync(Expression<Func<UserModel, bool>> predicate)
        {
            var queryUsers = _userDbContext.Users.Include(u => u.UserRoles).ThenInclude(r => r.Role);
            var queryUserModels = _mapper.ProjectTo<UserModel>(queryUsers, null).Where(predicate);
            var userModel = await queryUserModels.SingleOrDefaultAsync();
            return userModel;
        }        
    }
}

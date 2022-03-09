using AutoMapper;
using DataLayer.Contexts;
using DataLayer.Entities.Account;
using LogicLayer.InterfacesOut.Account;
using LogicLayer.Models;

namespace DataLayer.Repositories.Account
{
    public class UserRoleRepository : GenericRepository<UserRoleModel, UserRole>, IUserRoleRepository
    {
        private readonly AccountDbContext _userDbContext;

        public UserRoleRepository(IMapper mapper, AccountDbContext userDbContext)
            : base(mapper, userDbContext)
        {
            _userDbContext = userDbContext;
        }

        // Custom method implementations are added here
    }
}

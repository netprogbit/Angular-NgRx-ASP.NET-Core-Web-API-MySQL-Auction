using AutoMapper;
using DataLayer.Contexts;
using DataLayer.Repositories.Account;
using LogicLayer.InterfacesOut.Account;

namespace DataLayer.UnitOfWorks.Account
{
    public class AccountUnitOfWork : UnitOfWork, IAccountUnitOfWork
    {
        private readonly AccountDbContext _userDbContext;

        public AccountUnitOfWork(IMapper mapper, AccountDbContext userDbContext)
            : base(mapper, userDbContext)
        {
            _userDbContext = userDbContext;
        }

        private IUserRepository _userRepository;
        public IUserRepository Users => _userRepository ?? (_userRepository = new UserRepository(_mapper, _userDbContext));

        private IUserRoleRepository _userRoleRepository;
        public IUserRoleRepository UserRoles => _userRoleRepository ?? (_userRoleRepository = new UserRoleRepository(_mapper, _userDbContext));

        private IRoleRepository _roleRepository;
        public IRoleRepository Roles => _roleRepository ?? (_roleRepository = new RoleRepository(_mapper, _userDbContext));

    }
}

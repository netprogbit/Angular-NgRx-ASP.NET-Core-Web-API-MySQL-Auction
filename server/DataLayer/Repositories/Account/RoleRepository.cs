using AutoMapper;
using DataLayer.Contexts;
using DataLayer.Entities.Account;
using LogicLayer.InterfacesOut.Account;
using LogicLayer.Models;

namespace DataLayer.Repositories.Account
{
    public class RoleRepository : AccountGenericRepository<RoleModel, Role>, IRoleRepository
    {
        private readonly AccountDbContext _userDbContext;

        public RoleRepository(IMapper mapper, AccountDbContext userDbContext)
            : base(mapper, userDbContext)
        {
            _userDbContext = userDbContext;
        }

        // Custom method implementations are added here
    }
}

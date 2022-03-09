namespace LogicLayer.InterfacesOut.Account
{
    public interface IAccountUnitOfWork : IUnitOfWork
    {
        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        IRoleRepository Roles { get; }
    }
}

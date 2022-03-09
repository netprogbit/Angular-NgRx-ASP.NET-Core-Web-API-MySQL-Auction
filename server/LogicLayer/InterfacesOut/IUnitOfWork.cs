using System;
using System.Threading.Tasks;

namespace LogicLayer.InterfacesOut
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        Task SaveAsync();
    }
}

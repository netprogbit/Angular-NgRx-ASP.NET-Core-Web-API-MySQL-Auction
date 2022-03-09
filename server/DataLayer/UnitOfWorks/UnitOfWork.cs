using AutoMapper;
using LogicLayer.InterfacesOut;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace DataLayer.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly IMapper _mapper;
        protected readonly DbContext _dbContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(IMapper mapper, DbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;            
        }

        public void BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _dbContext.Dispose();
                _transaction?.Dispose();
            }

            _disposed = true;
        }        
    }
}

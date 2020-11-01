using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataLayer.Abstractions
{
    internal interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(long id);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task CreateAsync(T item);
        void Update(T item);
        bool Delete(long id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LogicLayer.InterfacesOut
{
	public interface IGenericRepository<TModel>
        where TModel : class
	{
        Task<IEnumerable<TModel>> FindAllAsync();
        Task<IEnumerable<TModel>> FindAllAsync(Expression<Func<TModel, bool>> predicate);        
        Task<TModel> FindAsync(Expression<Func<TModel, bool>> predicate);
        Task<TModel> AddAsync(TModel model);
        Task AddRangeAsync(IEnumerable<TModel> models);        
    }
}

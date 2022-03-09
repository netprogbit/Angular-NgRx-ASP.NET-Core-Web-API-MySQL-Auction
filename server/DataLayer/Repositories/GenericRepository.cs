using AutoMapper;
using LogicLayer.InterfacesOut;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
	/// <summary>
	/// Gineric repository
	/// </summary>
	public class GenericRepository<TModel, TEntity> : IGenericRepository<TModel>
        where TModel : class
        where TEntity : class
    {
		protected readonly IMapper _mapper;        
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(IMapper mapper, DbContext dbContext)
		{
			_mapper = mapper;            
            _dbSet = dbContext.Set<TEntity>();
        }
        
        public async Task<IEnumerable<TModel>> FindAllAsync()
        {
            var entites = await _dbSet.AsNoTracking().ToListAsync();
            var models = _mapper.Map<IEnumerable<TModel>>(entites);
            return models;
        }

        public async Task<IEnumerable<TModel>> FindAllAsync(Expression<Func<TModel, bool>> predicate)
        {
            var queryEntities = _dbSet.AsNoTracking();
            var queryUserModels = _mapper.ProjectTo<TModel>(queryEntities, null).Where(predicate);
            var models = await queryUserModels.ToListAsync();
            return models;
        }        

        public async Task<TModel> FindAsync(Expression<Func<TModel, bool>> predicate)
        {
            var queryEntities = _dbSet.AsNoTracking();
            var queryUserModels = _mapper.ProjectTo<TModel>(queryEntities, null).Where(predicate);
            var model = await queryUserModels.SingleOrDefaultAsync();
            return model;
        }

        public async Task<TModel> AddAsync(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            var result = await _dbSet.AddAsync(entity);
            var addedModel = _mapper.Map<TModel>(result.Entity);
            return addedModel;
        }

        public async Task AddRangeAsync(IEnumerable<TModel> models)
        {
            var entities = _mapper.Map<IEnumerable<TEntity>>(models);
            await _dbSet.AddRangeAsync(entities);
        }                
    }
}

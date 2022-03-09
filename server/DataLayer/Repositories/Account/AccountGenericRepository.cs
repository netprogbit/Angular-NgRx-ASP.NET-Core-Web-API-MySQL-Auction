using AutoMapper;
using DataLayer.Contexts;
using DataLayer.Entities.Interfaces;
using LogicLayer.InterfacesOut.Account;
using LogicLayer.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Account
{
    public class AccountGenericRepository<TModel, TEntity> : GenericRepository<TModel, TEntity>, IAccountGenericRepository<TModel>
        where TModel : class, IAccountModel 
        where TEntity : class, IAccountEntity
    {
        public AccountGenericRepository(IMapper mapper, AccountDbContext userDbContext)
            : base(mapper, userDbContext)
        {
        }

        public async Task<TModel> FindByIdAsync(string id)
        {
            var entity = await _dbSet.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
            var model = _mapper.Map<TModel>(entity);
            return model;
        }

        public void Update(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            _dbSet.Update(entity);
        }

        public async Task DeleteByIdAsync(string id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
        }
    }
}

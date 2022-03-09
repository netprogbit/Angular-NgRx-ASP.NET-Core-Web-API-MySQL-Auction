using AutoMapper;
using DataLayer.Contexts;
using DataLayer.Entities.Interfaces;
using LogicLayer.InterfacesOut.Auction;
using LogicLayer.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Auction
{
    public class AuctionGenericRepository<TModel, TEntity> : GenericRepository<TModel, TEntity>, IAuctionGenericRepository<TModel>
        where TModel : class, IAuctionModel 
        where TEntity : class, IAuctionEntity
    {
        public AuctionGenericRepository(IMapper mapper, AuctionDbContext auctionDbContext)
           : base(mapper, auctionDbContext)
        {
        }

        public async Task<TModel> FindByIdAsync(long id)
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

        public async Task DeleteByIdAsync(long id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
        }
    }
}

using LogicLayer.Models.Interfaces;
using System.Threading.Tasks;

namespace LogicLayer.InterfacesOut.Auction
{
    public interface IAuctionGenericRepository<TModel> : IGenericRepository<TModel>
        where TModel : class, IAuctionModel
    {
        Task<TModel> FindByIdAsync(long id);
        void Update(TModel model);
        Task DeleteByIdAsync(long id);
    }
}

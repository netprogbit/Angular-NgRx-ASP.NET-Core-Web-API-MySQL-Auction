using LogicLayer.Models;
using System.Threading.Tasks;

namespace LogicLayer.InterfacesOut.Auction
{
    public interface IProductRepository : IAuctionGenericRepository<ProductModel>
    {
        Task<PaginationModel<ProductModel>> GetProductPageAsync(string categoryName, string searchTerm, int pageIndex, int pageSize);
    }
}

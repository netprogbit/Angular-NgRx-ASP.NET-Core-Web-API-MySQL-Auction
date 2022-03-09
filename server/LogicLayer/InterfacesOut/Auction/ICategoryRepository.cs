using LogicLayer.Models;
using System.Threading.Tasks;

namespace LogicLayer.InterfacesOut.Auction
{
    public interface ICategoryRepository : IAuctionGenericRepository<CategoryModel>
    {
        Task<PaginationModel<CategoryModel>> GetCategoryPageAsync(string searchTerm, int pageIndex, int pageSize);
    }
}

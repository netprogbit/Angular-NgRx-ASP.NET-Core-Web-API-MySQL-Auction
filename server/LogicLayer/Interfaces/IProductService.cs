using LogicLayer.Models;
using System.Threading.Tasks;

namespace LogicLayer.Interfaces
{
    public interface IProductService
    {
        Task<ProductModel> GetProductByIdAsync(long id);
        Task<PaginationModel<ProductModel>> GetProductPageAsync(string categoryName, string searchTerm, int pageIndex, int pageSize);
        Task AddProductAsync(ProductModel productModel);
        Task UpdateProductAsync(ProductModel productModel);
        Task DeleteProductAsync(long id);
    }
}

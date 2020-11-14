using Microsoft.AspNetCore.Http;
using Server.Models;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IProductService
    {
        Task AddProductAsync(HttpRequest request);
        Task DeleteAsync(long id);
        Task<ProductResult> GetProductAsync(long id);
        Task<PaginationResult<ProductResult>> GetProductsAsync(string categoryName, string searchTerm, int pageIndex, int pageSize);
        Task UpdateAsync(HttpRequest request);
    }
}
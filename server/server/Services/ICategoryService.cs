using Microsoft.AspNetCore.Http;
using Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(HttpRequest request);
        Task<List<CategoryResult>> GetAllCategoriesAsync();
        Task<PaginationResult<CategoryResult>> GetCategoriesAsync(string searchTerm, int pageIndex, int pageSize);
        Task UpdateCategoryAsync(HttpRequest request);
    }
}
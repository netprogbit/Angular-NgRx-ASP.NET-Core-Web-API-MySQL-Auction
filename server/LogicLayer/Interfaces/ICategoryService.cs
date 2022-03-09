using LogicLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicLayer.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync();
        Task<PaginationModel<CategoryModel>> GetCategoryPageAsync(string searchTerm, int pageIndex, int pageSize);
        Task AddCategoryAsync(CategoryModel categoryModel);
        Task UpdateCategoryAsync(CategoryModel categoryModel);

    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    /// <summary>
    /// Category actions
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get categories completely
        /// </summary>
        [HttpGet("allcategories")]
        public async Task<List<CategoryResult>> GetAllCategories() => await _categoryService.GetAllCategoriesAsync();

        /// <summary>
        /// Getting categories according to pagination
        /// </summary>
        /// <param name="searchTerm">for term selection</param>
        /// <param name="pageIndex">for pagination</param>
        /// <param name="pageSize">for pagination</param>
        /// <returns>Category pagination data</returns>
        [HttpGet("categories")]
        public async Task<PaginationResult<CategoryResult>> GetCategories(string searchTerm = "", int pageIndex = 0, int pageSize = 10) => await _categoryService.GetCategoriesAsync(searchTerm, pageIndex, pageSize);

        /// <summary>
        /// Create category
        /// </summary>
        /// <returns>OK message</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("")]
        public async Task<IActionResult> Add()
        {
            await _categoryService.AddCategoryAsync(Request);
            return Ok(new { message = StringHelper.AdditionSuccefully });
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <returns>Success message</returns>
        [Authorize(Roles = "admin")]
        [HttpPut("")]
        public async Task<IActionResult> Update()
        {
            await _categoryService.UpdateCategoryAsync(Request);
            return Ok(new { message = StringHelper.UpdationSuccefully });
        }
    }
}

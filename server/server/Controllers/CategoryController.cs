using AutoMapper;
using LogicLayer.Helpers;
using LogicLayer.Interfaces;
using LogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    /// <summary>
    /// Category actions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CategoryController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get categories completely
        /// </summary>
        [HttpGet("allcategories")]
        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var categoryModels = await _categoryService.GetAllCategoriesAsync();
            var categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categoryModels);
            return categoryDTOs;
        }

        /// <summary>
        /// Getting categories according to pagination
        /// </summary>
        /// <param name="searchTerm">for term selection</param>
        /// <param name="pageIndex">for pagination</param>
        /// <param name="pageSize">for pagination</param>
        /// <returns>Category pagination data</returns>
        [HttpGet("categories")]
        public async Task<PaginationDTO<CategoryDTO>> GetCategoryPage(string searchTerm = "", int pageIndex = 0, int pageSize = 10)
        {
            var paginationModel = await _categoryService.GetCategoryPageAsync(searchTerm, pageIndex, pageSize);
            var paginationDTO = _mapper.Map<PaginationDTO<CategoryDTO>>(paginationModel);
            return paginationDTO;
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <returns>OK message</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        public async Task<IActionResult> AddCategory([FromForm] CategoryDTO categoryDTO)
        {
            var categoryModel = _mapper.Map<CategoryModel>(categoryDTO);
            await _categoryService.AddCategoryAsync(categoryModel);
            return Ok(StringHelper.AdditionSuccessfully);
        }


        /// <summary>
        /// Update category
        /// </summary>
        /// <returns>Success message</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("")]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryDTO categoryDTO)
        {
            var categoryModel = _mapper.Map<CategoryModel>(categoryDTO);
            await _categoryService.UpdateCategoryAsync(categoryModel);
            return Ok(StringHelper.UpdationSuccessfully);
        }
    }
}

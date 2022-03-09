using AutoMapper;
using LogicLayer.Helpers;
using LogicLayer.Interfaces;
using LogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using System.Threading.Tasks;

namespace Server.Controllers
{
    /// <summary>
    /// Product actions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        /// <summary>
        /// Getting product by Id
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>Product for client</returns>
        [HttpGet("{id}")]
        public async Task<ProductDTO> GetProduct(long id)
        {
            var productModel = await _productService.GetProductByIdAsync(id);
            var productDTO = _mapper.Map<ProductDTO>(productModel);
            return productDTO;
        }

        /// <summary>
        /// Getting products according to pagination
        /// </summary>
        /// <param name="categoryName">for category selection</param>
        /// <param name="searchTerm">for term selection</param>
        /// <param name="pageIndex">for pagination</param>
        /// <param name="pageSize">for pagination</param>
        /// <returns>Product pagination data</returns>
        [HttpGet("products")]
        public async Task<PaginationDTO<ProductDTO>> GetProductPage(string categoryName = "", string searchTerm = "", int pageIndex = 0, int pageSize = 10)
        {
            var paginationModel = await _productService.GetProductPageAsync(categoryName, searchTerm, pageIndex, pageSize);
            var paginationDTO = _mapper.Map<PaginationDTO<ProductDTO>>(paginationModel);
            return paginationDTO;
        }

        /// <summary>
        /// Create product
        /// </summary>
        /// <returns>OK message</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDTO productDTO)
        {
            var productModel = _mapper.Map<ProductModel>(productDTO);
            await _productService.AddProductAsync(productModel);
            return Ok(StringHelper.AdditionSuccessfully);
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <returns>Success message</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductDTO productDTO)
        {
            var productModel = _mapper.Map<ProductModel>(productDTO);
            await _productService.UpdateProductAsync(productModel);
            return Ok(StringHelper.UpdationSuccessfully);
        }

        /// <summary>
        /// Delete product by Id
        /// </summary>
        /// <returns>Succcess message</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok(StringHelper.DeleteSuccefully);
        }
    }
}

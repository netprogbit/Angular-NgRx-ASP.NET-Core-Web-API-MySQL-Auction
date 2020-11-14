using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    /// <summary>
    /// Product actions
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IAuctionService _auctionService;

        public ProductController(IProductService productService, IAuctionService auctionService)
        {
            _productService = productService;
            _auctionService = auctionService;
        }

        /// <summary>
        /// Getting product by Id
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>Product for client</returns>
        [HttpGet("{id}")]
        public async Task<ProductResult> GetProduct(long id) => await _productService.GetProductAsync(id);

        /// <summary>
        /// Getting products according to pagination
        /// </summary>
        /// <param name="categoryName">for category selection</param>
        /// <param name="searchTerm">for term selection</param>
        /// <param name="pageIndex">for pagination</param>
        /// <param name="pageSize">for pagination</param>
        /// <returns>Product pagination data</returns>
        [HttpGet("products")]
        public async Task<PaginationResult<ProductResult>> GetProducts(string categoryName = "", string searchTerm = "", int pageIndex = 0, int pageSize = 10) => await _productService.GetProductsAsync(categoryName, searchTerm, pageIndex, pageSize);

        /// <summary>
        /// Bid offer
        /// </summary>
        /// <returns>Bid price</returns>
        [HttpPost("buy")]
        public async Task<IActionResult> Buy(Buy buy)
        {
            decimal price = await _auctionService.Buy(buy.UserId, buy.ProductId);
            return Ok(new { message = StringHelper.PurchaseRequestAccepted + price });
        }

        /// <summary>
        /// Create product
        /// </summary>
        /// <returns>OK message</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("")]
        public async Task<IActionResult> Add()
        {
            await _productService.AddProductAsync(Request);
            return Ok(new { message = StringHelper.AdditionSuccefully });
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <returns>Success message</returns>
        [Authorize(Roles = "admin")]
        [HttpPut("")]
        public async Task<IActionResult> Update()
        {
            await _productService.UpdateAsync(Request);
            return Ok(new { message = StringHelper.UpdationSuccefully });
        }

        /// <summary>
        /// Delete product by Id
        /// </summary>
        /// <returns>Succcess message</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _productService.DeleteAsync(id);
            return Ok(new { message = StringHelper.DeleteSuccefully });
        }        
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    protected readonly ILogger<ProductController> _logger;
    private readonly UnitOfWork _unitOfWork;
    private readonly AuctionService _auctionService;

    public ProductController(ILogger<ProductController> logger, UnitOfWork unitOfWork, AuctionService auctionService)
    {
      _logger = logger;
      _unitOfWork = unitOfWork;
      _auctionService = auctionService;
    }

    /// <summary>
    /// Getting product by Id
    /// </summary>
    /// <param name="id">Product Id</param>
    /// <returns>Product for client</returns>
    [HttpGet("{id}")]
    public async Task<ProductResult> GetProduct(long id)
    {
      Product product = await _unitOfWork.Products.GetAsync(id);
      return new ProductResult(product.Id, new CategoryResult(product.Category.Id, product.Category.Name, product.Category.ImageFileName), 
          product.Name, product.Description, PriceHelper.IntToDecimal(product.Price), PriceHelper.IntToDecimal(product.SellerPrice), product.ImageFileName, product.Bidder);
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
    public async Task<PaginationResult<ProductResult>> GetProducts(string categoryName = "", string searchTerm = "", int pageIndex = 0, int pageSize = 10)
    {
      IQueryable<Product> products = _unitOfWork.Products.GetAll().Where(p => p.Category.Name.Contains(categoryName)).Where(p => p.Name.Contains(searchTerm)).OrderBy(p => p.Id);
      var count = await products.CountAsync();
      var items = await products.Skip(pageIndex * pageSize).Take(pageSize).Select(p => new ProductResult(p.Id, new CategoryResult(p.Category.Id, p.Category.Name, p.Category.ImageFileName), 
          p.Name, p.Description, PriceHelper.IntToDecimal(p.Price), PriceHelper.IntToDecimal(p.SellerPrice), p.ImageFileName, p.Bidder)).ToListAsync();

      return new PaginationResult<ProductResult>(items, count);
    }

    /// <summary>
    /// Bid offer
    /// </summary>
    /// <returns>Bid price</returns>
    [HttpPost("buy")]
    public async Task<IActionResult> Buy()
    {
      decimal price = await _auctionService.Buy(Int32.Parse(Request.Form["userId"]), Int32.Parse(Request.Form["productId"]));
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
      // Addition
      IFormFile file = Request.Form.Files.FirstOrDefault();
      string newFileName = FileHelper.GetUniqueFileName(file?.FileName);
      await AddProductAsync(Request, newFileName);
      await FileHelper.AddImageAsync(file, newFileName);
      await _auctionService.StartSaleAsync(Int64.Parse(Request.Form["id"]), PriceHelper.StrToInt(Request.Form["price"])); // Statring sale
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
      IFormFile file = Request.Form.Files.FirstOrDefault();
      string newFileName = FileHelper.GetUniqueFileName(file?.FileName);
      string oldFileName = await UpdateProductAsync(Request, newFileName);
      await FileHelper.UpdateImageAsync(file, oldFileName, newFileName);
      await _auctionService.StartSaleAsync(Int64.Parse(Request.Form["id"]), PriceHelper.StrToInt(Request.Form["price"])); // Statring sale
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
      Product product = await _unitOfWork.Products.GetAll().AsNoTracking().SingleOrDefaultAsync(p => p.Id == id);
      await DeleteProductAsync(id);  // Delete product from DB      
      FileHelper.DeleteFile(product.ImageFileName); // Detele unnecessary image file            
      await _auctionService.StopSale(id); // Stopping sale      
      User userBidder = await _unitOfWork.Users.GetAsync(product.Bidder);

      if (userBidder != null)
      {
        _logger.LogError("{0} User ID: {1}. Product info:  {2} | {3} | ${4}, ", StringHelper.EmailMessageNotSent, product.Bidder, product.Name, product.Description, PriceHelper.IntToDecimal(product.Price));
        EmailHelper.Send(userBidder.Email); // Send info to the buyer about the purchase of the product
      }

      return Ok(new { message = StringHelper.DeleteSuccefully });
    }

    private async Task AddProductAsync(HttpRequest request, string newFileName)
    {
      //Product adding DB transaction
      using (var dbContextTransaction = _unitOfWork.BeginTransaction())
      {
        try
        {
          Product product = new Product();
          Category category = await _unitOfWork.Categories.GetAll().AsNoTracking().SingleOrDefaultAsync(c => c.Name == request.Form["categoryName"]);
          product.CategoryId = category.Id;
          product.Name = request.Form["name"];
          product.Description = request.Form["description"];
          product.Price = PriceHelper.StrToInt(request.Form["price"]);
          product.SellerPrice = product.Price;
          product.ImageFileName = FileHelper.FilterImageName(newFileName);
          product.Bidder = 0;
          await _unitOfWork.Products.CreateAsync(product);
          await _unitOfWork.SaveAsync();
          dbContextTransaction.Commit();
        }
        catch (Exception e)
        {
          dbContextTransaction.Rollback(); // Rollbacking DB      
          throw new ApplicationException("DB Transaction Failed. " + e.Message);
        }
      }
    }

    private async Task<string> UpdateProductAsync(HttpRequest request, string newFileName)
    {
      string oldFileName = null;

      // Product updating DB transaction
      using (var dbContextTransaction = _unitOfWork.BeginTransaction())
      {
        try
        {
          Product product = await _unitOfWork.Products.GetAll().AsNoTracking().SingleOrDefaultAsync(p => p.Id == Int64.Parse(request.Form["id"]));
          oldFileName = product.ImageFileName;
          Category category = await _unitOfWork.Categories.GetAll().AsNoTracking().SingleOrDefaultAsync(c => c.Name == request.Form["categoryName"]);
          product.CategoryId = category.Id;
          product.Name = request.Form["name"];
          product.Description = request.Form["description"];
          product.Price = PriceHelper.StrToInt(request.Form["price"]);
          product.SellerPrice = product.Price;

          if (!string.IsNullOrEmpty(newFileName))
            product.ImageFileName = newFileName;

          product.Bidder = 0;
          _unitOfWork.Products.Update(product);

          await _unitOfWork.SaveAsync();
          dbContextTransaction.Commit();
        }
        catch (Exception e)
        {
          dbContextTransaction.Rollback(); // Rollbacking DB       
          throw new ApplicationException("DB Transaction Failed. " + e.Message);
        }
      }

      return oldFileName;
    }

    private async Task DeleteProductAsync(long id)
    {
      // Product deleting DB transaction
      using (var dbContextTransaction = _unitOfWork.BeginTransaction())
      {
        try
        {
          _unitOfWork.Products.Delete(id);
          await _unitOfWork.SaveAsync();
          dbContextTransaction.Commit();
        }
        catch (Exception e)
        {
          dbContextTransaction.Rollback(); // Rollbacking DB       
          throw new ApplicationException("DB Transaction Failed. " + e.Message);
        }
      }
    }

  }
}

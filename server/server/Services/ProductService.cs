using DataLayer.Entities;
using DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Server.Helpers;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuctionService _auctionService;
        protected readonly ILogger<ProductService> _logger;

        public ProductService(IUnitOfWork unitOfWork, IAuctionService auctionService, ILogger<ProductService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _auctionService = auctionService;
        }

        public async Task<ProductResult> GetProductAsync(long id)
        {
            Product product = await _unitOfWork.Products.FindAsync(id);
            return new ProductResult(product.Id, new CategoryResult(product.Category.Id, product.Category.Name, product.Category.ImageFileName),
                product.Name, product.Description, PriceHelper.IntToDecimal(product.Price), PriceHelper.IntToDecimal(product.SellerPrice), product.ImageFileName, product.Bidder);
        }

        public async Task<PaginationResult<ProductResult>> GetProductsAsync(string categoryName, string searchTerm, int pageIndex, int pageSize)
        {
            IEnumerable<Product> products = await _unitOfWork.Products.FindAllAsync(p => p.Category.Name.Contains(categoryName) && p.Name.Contains(searchTerm));
            var count = products.Count();
            var items = products.Skip(pageIndex * pageSize).Take(pageSize).Select(p => new ProductResult(p.Id, new CategoryResult(p.Category.Id, p.Category.Name, p.Category.ImageFileName),
                p.Name, p.Description, PriceHelper.IntToDecimal(p.Price), PriceHelper.IntToDecimal(p.SellerPrice), p.ImageFileName, p.Bidder)).ToList();

            return new PaginationResult<ProductResult>(items, count);
        }

        public async Task AddProductAsync(HttpRequest request)
        {
            // Addition
            IFormFile file = request.Form.Files.FirstOrDefault();
            string newFileName = FileHelper.GetUniqueFileName(file?.FileName);

            //Product adding DB transaction
            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Product product = new Product();
                    Category category = await _unitOfWork.Categories.FindAsync(c => c.Name == request.Form["categoryName"]);
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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback(); // Rollbacking DB      
                    ExceptionDispatchInfo.Capture(ex).Throw();
                }
            }

            await FileHelper.AddImageAsync(file, newFileName);
            await _auctionService.StartSaleAsync(Int64.Parse(request.Form["id"]), PriceHelper.StrToInt(request.Form["price"])); // Statring sale            
        }

        public async Task UpdateAsync(HttpRequest request)
        {
            IFormFile file = request.Form.Files.FirstOrDefault();
            string newFileName = FileHelper.GetUniqueFileName(file?.FileName);
            string oldFileName = null;

            // Product updating DB transaction
            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Product product = await _unitOfWork.Products.FindAsync(Int64.Parse(request.Form["id"]));
                    oldFileName = product.ImageFileName;
                    Category category = await _unitOfWork.Categories.FindAsync(c => c.Name == request.Form["categoryName"]);
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
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback(); // Rollbacking DB       
                    ExceptionDispatchInfo.Capture(ex).Throw();
                }
            }


            await FileHelper.UpdateImageAsync(file, oldFileName, newFileName);
            await _auctionService.StartSaleAsync(Int64.Parse(request.Form["id"]), PriceHelper.StrToInt(request.Form["price"])); // Statring sale            
        }

        public async Task DeleteAsync(long id)
        {
            Product product = await _unitOfWork.Products.FindAsync(id);

            // Product deleting DB transaction
            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _unitOfWork.Products.Delete(id);
                    await _unitOfWork.SaveAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback(); // Rollbacking DB       
                    ExceptionDispatchInfo.Capture(ex).Throw();
                }
            }

            FileHelper.DeleteFile(product.ImageFileName); // Detele unnecessary image file            
            await _auctionService.StopSale(id); // Stopping sale      
            User userBidder = await _unitOfWork.Users.FindAsync(product.Bidder);

            if (userBidder != null)
            {
                _logger.LogError("{0} User ID: {1}. Product info:  {2} | {3} | ${4}, ", StringHelper.EmailMessageNotSent, product.Bidder, product.Name, product.Description, PriceHelper.IntToDecimal(product.Price));
                EmailHelper.Send(userBidder.Email); // Send info to the buyer about the purchase of the product
            }
        }
    }
}

using LogicLayer.Helpers;
using LogicLayer.Interfaces;
using LogicLayer.InterfacesOut.Auction;
using LogicLayer.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using LogicLayer.InterfacesOut.Account;
using Microsoft.Extensions.Logging;

namespace LogicLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IAuctionUnitOfWork _auctionUnitOfWork;
        private readonly IAccountUnitOfWork _accountUnitOfWork;
        private readonly IAuctionService _auctionService;

        public ProductService(ILogger<ProductService> logger, IAuctionUnitOfWork auctionUnitOfWork, IAccountUnitOfWork accountUnitOfWork, IAuctionService auctionService)
        {
            _logger = logger;
            _auctionUnitOfWork = auctionUnitOfWork;
            _accountUnitOfWork = accountUnitOfWork;
            _auctionService = auctionService;
        }

        public async Task<ProductModel> GetProductByIdAsync(long id)
        {
            var productModel = await _auctionUnitOfWork.Products.FindByIdAsync(id);
            return productModel;
        }

        public async Task<PaginationModel<ProductModel>> GetProductPageAsync(string categoryName, string searchTerm, int pageIndex, int pageSize)
        {
            var paginationModel = await _auctionUnitOfWork.Products.GetProductPageAsync(categoryName, searchTerm, pageIndex, pageSize);
            return paginationModel;
        }

        public async Task AddProductAsync(ProductModel productModel)
        {
            IFormFile file = productModel.Image;
            string newFileName = FileHelper.GetUniqueFileName(file?.FileName);
            var category = await _auctionUnitOfWork.Categories.FindAsync(c => c.Name == productModel.CategoryName);
            productModel.CategoryId = category.Id;
            productModel.ImageFileName = FileHelper.FilterImageName(newFileName);
            productModel.BidderEmail = null;
            await _auctionUnitOfWork.Products.AddAsync(productModel);
            await _auctionUnitOfWork.SaveAsync();
            await FileHelper.AddImageAsync(file, newFileName);
            await _auctionService.StartSaleAsync(productModel.Id, productModel.Price); // Statring sale            
        }

        public async Task UpdateProductAsync(ProductModel productModel)
        {
            IFormFile file = productModel.Image;
            string newFileName = FileHelper.GetUniqueFileName(file?.FileName);
            string oldFileName = null;
            var currentProductModel = await _auctionUnitOfWork.Products.FindByIdAsync(productModel.Id);
            oldFileName = currentProductModel.ImageFileName;
            var category = await _auctionUnitOfWork.Categories.FindAsync(c => c.Name == productModel.CategoryName);
            currentProductModel.Category = category;
            currentProductModel.CategoryId = category.Id;
            currentProductModel.Name = productModel.Name;
            currentProductModel.Description = productModel.Description;
            currentProductModel.Price = productModel.Price;
            currentProductModel.SellerPrice = currentProductModel.Price;

            if (!string.IsNullOrEmpty(newFileName))
                currentProductModel.ImageFileName = newFileName;

            currentProductModel.BidderEmail = null;
            _auctionUnitOfWork.Products.Update(currentProductModel);
            await _auctionUnitOfWork.SaveAsync();
            await FileHelper.UpdateImageAsync(file, oldFileName, newFileName);
            await _auctionService.StartSaleAsync(productModel.Id, productModel.Price); // Statring sale            
        }

        public async Task DeleteProductAsync(long id)
        {
            var productModel = await _auctionUnitOfWork.Products.FindByIdAsync(id);
            await _auctionUnitOfWork.Products.DeleteByIdAsync(id);
            await _auctionUnitOfWork.SaveAsync();
            FileHelper.DeleteFile(productModel.ImageFileName); // Detele unnecessary image file            
            await _auctionService.StopSaleAsync(id); // Stopping sale      
            var userBidder = await _accountUnitOfWork.Users.FindAsync(u => u.Email == productModel.BidderEmail);

            if (userBidder != null)
            {
                _logger.LogError("{0} Email: {1}. Product info:  {2} | {3} | ${4}, ", StringHelper.EmailMessageNotSent, productModel.BidderEmail, productModel.Name, productModel.Description, productModel.Price);
                EmailHelper.Send(userBidder.Email); // Send info to the buyer about the purchase of the product
            }
        }
    }
}

using LogicLayer.Helpers;
using LogicLayer.Hubs;
using LogicLayer.Interfaces;
using LogicLayer.InterfacesOut.Account;
using LogicLayer.InterfacesOut.Auction;
using LogicLayer.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LogicLayer.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IHubContext<PriceHub> _priceHub;
        private readonly IAuctionUnitOfWork _auctionUnitOfWork;
        private readonly IAccountUnitOfWork _accountUnitOfWork;

        public AuctionService(IHubContext<PriceHub> priceHub, IAuctionUnitOfWork auctionUnitOfWork, IAccountUnitOfWork accountUnitOfWork)
        {
            _priceHub = priceHub;
            _auctionUnitOfWork = auctionUnitOfWork;
            _accountUnitOfWork = accountUnitOfWork;
        }

        /// <summary>
        /// Start of sale
        /// </summary>
        public async Task StartSaleAsync(long productId, int price)
        {
            await _priceHub.Clients.All.SendAsync(productId.ToString(), PriceHelper.IntToDecimal(price)); // Potential buyer price notification
        }

        /// <summary>
        /// Bid offer
        /// </summary>
        public async Task<BuyModel> BuyAsync(BuyModel buyModel)
        {
            var productModel = await _auctionUnitOfWork.Products.FindByIdAsync(buyModel.ProductId);
            productModel.Price = productModel.SellerPrice;
            productModel.SellerPrice = (int)(productModel.Price * 1.1); // Increase by 10%
            var userModel = await _accountUnitOfWork.Users.FindByIdAsync(buyModel.UserId);
            productModel.BidderEmail = userModel.Email;
            _auctionUnitOfWork.Products.Update(productModel);
            await _auctionUnitOfWork.SaveAsync();
            await _priceHub.Clients.All.SendAsync(buyModel.ProductId.ToString(), productModel.SellerPrice);
            buyModel.Price = productModel.Price;
            return buyModel;
        }

        /// <summary>
        /// End of sale
        /// </summary>
        public async Task StopSaleAsync(long productId)
        {
            await _priceHub.Clients.All.SendAsync(productId.ToString(), 0.00M); // Product sale is over
        }
    }
}

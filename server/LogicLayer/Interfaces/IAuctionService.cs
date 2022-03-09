using LogicLayer.Models;
using System.Threading.Tasks;

namespace LogicLayer.Interfaces
{
    public interface IAuctionService
    {
        Task StartSaleAsync(long productId, int price);
        Task<BuyModel> BuyAsync(BuyModel buyModel);
        Task StopSaleAsync(long productId);
    }
}

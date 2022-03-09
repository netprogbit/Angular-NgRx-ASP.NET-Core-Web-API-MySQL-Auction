using LogicLayer.Helpers;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LogicLayer.Hubs
{
    /// <summary>
    /// SignalR auction hub
    /// </summary>
    public class PriceHub : Hub
    {
        public async Task SendAsync(long productId, int price)
        {
            await Clients.All.SendAsync(productId.ToString(), PriceHelper.IntToDecimal(price));
        }
    }
}

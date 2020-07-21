using Microsoft.AspNetCore.SignalR;
using Server.Helpers;
using System.Threading.Tasks;

namespace Server.Hubs
{
  /// <summary>
  /// SignalR auction hub
  /// </summary>
  public class AuctionHub : Hub
  {
    public async Task SendAsync(long productId, int price)
    {
      await Clients.All.SendAsync(productId.ToString(), PriceHelper.IntToDecimal(price));
    }
  }
}

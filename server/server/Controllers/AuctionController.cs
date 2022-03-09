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
    /// Auction actions
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuctionService _auctionService;

        public AuctionController(IMapper mapper, IAuctionService auctionService)
        {
            _mapper = mapper;
            _auctionService = auctionService;
        }

        /// <summary>
        /// Bid offer
        /// </summary>
        /// <returns>Bid price</returns>
        [HttpPost("")]
        public async Task<IActionResult> BuyProduct([FromBody] BuyDTO buyDTO)
        {
            var buyModel = _mapper.Map<BuyModel>(buyDTO);
            var newBuyModel = await _auctionService.BuyAsync(buyModel);
            var newBuyDTO = _mapper.Map<BuyDTO>(newBuyModel);
            return Ok($"{StringHelper.PurchaseRequestAccepted}{newBuyDTO.Price}");
        }
    }
}

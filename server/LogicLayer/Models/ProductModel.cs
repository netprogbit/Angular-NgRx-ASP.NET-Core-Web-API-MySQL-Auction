using LogicLayer.Models.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LogicLayer.Models
{
    public class ProductModel : IAuctionModel
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int SellerPrice { get; set; }
        public string ImageFileName { get; set; }
        public string BidderEmail { get; set; }
        public string CategoryName { get; set; }
        public CategoryModel Category { get; set; }
        public IFormFile Image { get; set; }
    }
}

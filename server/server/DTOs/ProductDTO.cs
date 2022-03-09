using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Server.DTOs
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal SellerPrice { get; set; }
        public string ImageFileName { get; set; }
        public string BidderEmail { get; set; }
        public string CategoryName { get; set; }
        public CategoryDTO Category { get; set; }
        public IFormFile Image { get; set; }
    }
}

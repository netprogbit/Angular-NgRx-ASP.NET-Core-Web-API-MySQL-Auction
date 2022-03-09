using LogicLayer.Models.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LogicLayer.Models
{
    public class CategoryModel : IAuctionModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageFileName { get; set; }
        public IFormFile Image { get; set; }
    }
}

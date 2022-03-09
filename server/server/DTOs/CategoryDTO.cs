using Microsoft.AspNetCore.Http;

namespace Server.DTOs
{
    public class CategoryDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageFileName { get; set; }
        public IFormFile Image { get; set; }
    }
}

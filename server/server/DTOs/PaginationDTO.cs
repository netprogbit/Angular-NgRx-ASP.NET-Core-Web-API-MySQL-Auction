using System.Collections.Generic;

namespace Server.DTOs
{
    public class PaginationDTO<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public int Length { get; set; }
    }
}

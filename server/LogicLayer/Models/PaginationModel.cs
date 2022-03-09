using System.Collections.Generic;

namespace LogicLayer.Models
{
    public class PaginationModel<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public int Length { get; set; }
    }
}

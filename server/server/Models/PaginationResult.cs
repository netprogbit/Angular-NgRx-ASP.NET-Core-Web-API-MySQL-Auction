using System.Collections.Generic;

namespace Server.Models
{
  public class PaginationResult<T> where T : class
  {
    public List<T> Items { get; set; }
    public int Length { get; set; }

    public PaginationResult()
    { }

    public PaginationResult(List<T> items, int length)
    {
      Items = items;
      Length = length;
    }
  }
}

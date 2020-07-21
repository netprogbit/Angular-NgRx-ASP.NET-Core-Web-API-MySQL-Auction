namespace Server.Models
{
  public class ProductResult
  {
    public long Id { get; set; }
    public CategoryResult Category { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal SellerPrice { get; set; }
    public string ImageFileName { get; set; }
    public long Bidder { get; set; }

    public ProductResult(long id, CategoryResult category, string name, string description, decimal price, decimal sellerPrice, string imageFileName, long bidder)
    {
      Id = id;
      Category = category;
      Name = name;
      Description = description;
      Price = price;
      SellerPrice = sellerPrice;
      ImageFileName = imageFileName;
      Bidder = bidder;
    }
  }
}

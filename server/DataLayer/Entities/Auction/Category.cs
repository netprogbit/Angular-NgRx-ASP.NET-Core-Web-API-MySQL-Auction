using DataLayer.Entities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.Auction
{
    public class Category : IAuctionEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }        
        public string Name { get; set; }
        public string ImageFileName { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

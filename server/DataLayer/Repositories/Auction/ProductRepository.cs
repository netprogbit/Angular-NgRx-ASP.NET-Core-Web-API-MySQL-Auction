using AutoMapper;
using DataLayer.Contexts;
using DataLayer.Entities.Auction;
using LogicLayer.InterfacesOut.Auction;
using LogicLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Auction
{
    public class ProductRepository : AuctionGenericRepository<ProductModel, Product>, IProductRepository
    {
        private readonly AuctionDbContext _auctionDbContext;

        public ProductRepository(IMapper mapper, AuctionDbContext auctionDbContext)
            : base(mapper, auctionDbContext)
        {
            _auctionDbContext = auctionDbContext;
        }

        public async Task<PaginationModel<ProductModel>> GetProductPageAsync(string categoryName, string searchTerm, int pageIndex, int pageSize)
        {
            var queryProducts = _auctionDbContext.Products.AsNoTracking().Include(p => p.Category).Where(p => p.Category.Name.Contains(categoryName) && p.Name.Contains(searchTerm));
            int count = await queryProducts.CountAsync();
            var querySelectedProducts = queryProducts.OrderBy(p => p.Id).Skip(pageIndex * pageSize).Take(pageSize);
            var selectedProducts = await querySelectedProducts.ToListAsync();
            var productModels = _mapper.Map<IEnumerable<ProductModel>>(selectedProducts);
            return new PaginationModel<ProductModel> { Items = productModels, Length = count };
        }
    }
}

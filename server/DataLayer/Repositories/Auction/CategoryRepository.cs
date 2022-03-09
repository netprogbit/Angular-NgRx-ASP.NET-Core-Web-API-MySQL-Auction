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
    public class CategoryRepository : AuctionGenericRepository<CategoryModel, Category>, ICategoryRepository
    {
        private readonly AuctionDbContext _auctionDbContext;

        public CategoryRepository(IMapper mapper, AuctionDbContext auctionDbContext)
            : base(mapper, auctionDbContext)
        {
            _auctionDbContext = auctionDbContext;
        }

        public async Task<PaginationModel<CategoryModel>> GetCategoryPageAsync(string searchTerm, int pageIndex, int pageSize)
        {
            var queryCategories = _auctionDbContext.Categories.AsNoTracking().Where(c => c.Name.Contains(searchTerm));
            int count = await queryCategories.CountAsync();
            var querySelectedCategories = queryCategories.OrderBy(u => u.Name).Skip(pageIndex * pageSize).Take(pageSize);
            var selectedCategories = await querySelectedCategories.ToListAsync();
            var categoryModels = _mapper.Map<IEnumerable<CategoryModel>>(selectedCategories);
            return new PaginationModel<CategoryModel> { Items = categoryModels, Length = count };
        }
    }
}

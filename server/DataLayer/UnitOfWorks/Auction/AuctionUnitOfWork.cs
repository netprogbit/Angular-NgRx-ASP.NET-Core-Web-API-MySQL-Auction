using AutoMapper;
using DataLayer.Contexts;
using DataLayer.Repositories.Auction;
using LogicLayer.InterfacesOut.Auction;

namespace DataLayer.UnitOfWorks.Auction
{
    public class AuctionUnitOfWork : UnitOfWork, IAuctionUnitOfWork
    {
        private readonly AuctionDbContext _auctionDbContext;

        public AuctionUnitOfWork(IMapper mapper, AuctionDbContext auctionDbContext)
            : base(mapper, auctionDbContext)
        {
            _auctionDbContext = auctionDbContext;
        }

        private IProductRepository _productPepository;
        public IProductRepository Products => _productPepository ?? (_productPepository = new ProductRepository(_mapper, _auctionDbContext));

        private ICategoryRepository _categoryRepository;
        public ICategoryRepository Categories => _categoryRepository ?? (_categoryRepository = new CategoryRepository(_mapper, _auctionDbContext));
    }
}

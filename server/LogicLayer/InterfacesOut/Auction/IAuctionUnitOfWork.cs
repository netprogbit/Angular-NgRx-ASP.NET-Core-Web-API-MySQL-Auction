namespace LogicLayer.InterfacesOut.Auction
{
    public interface IAuctionUnitOfWork : IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
    }
}

using DataAccess.Context;
using DataAccess.Repositories.Implement;
using DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IItemImageRepository ItemImage { get; private set; }
        public IFeedbackRepository Feedback { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IOrderRepository Order { get; private set; }
        private ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public UnitOfWork(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            ItemImage = new ItemImageRepository(_db, _configuration);
            Feedback = new FeedbackRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            Order = new OrderRepository(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

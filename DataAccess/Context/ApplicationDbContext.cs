using System.Reflection.Emit;
using DataAccess.Entities.Application;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Relation 1-1 between  Product and ProductAvatar
            builder
                .Entity<Product>()
                .HasOne(p => p.ProductAvatar)
                .WithOne()
                .HasForeignKey<Product>("ProductAvatarId")
                .OnDelete(DeleteBehavior.SetNull);

            // Relation 1-N between  Product and ProductImage
            builder
                .Entity<Product>()
                .HasMany(p => p.ProductImages)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

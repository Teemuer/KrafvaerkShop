using Krafvaerk_shop.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Krafvaerk_shop.Data
{
    public class ShopDBContext : DbContext
    {
        public ShopDBContext(DbContextOptions<ShopDBContext> options): base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<OrderRow> OrderRow { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<ProductCategory> ProductCategorie { get; set; }
    }
}

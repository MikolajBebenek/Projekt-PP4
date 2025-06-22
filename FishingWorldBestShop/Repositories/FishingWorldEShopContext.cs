using FishingWorldEShop.Models;
using Microsoft.EntityFrameworkCore;
namespace FishingWorldEShop.Repositories
{
    public class FishingWorldEShopContext : DbContext
    {
        public FishingWorldEShopContext(DbContextOptions<FishingWorldEShopContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
    }
}

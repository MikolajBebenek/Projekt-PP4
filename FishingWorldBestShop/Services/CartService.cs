using FishingWorldEShop.Models;
using FishingWorldEShop.Repositories;
using FishingWorldEShop.Services;
using Microsoft.EntityFrameworkCore;

namespace FishingWorldEShop
{
    public class CartService : ICartService
    {
        private readonly FishingWorldEShopContext _context;

        public CartService(FishingWorldEShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync() =>
            await _context.CartItems
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.Category)
                .Include(ci => ci.Customer)
                .ToListAsync();

        public async Task<CartItem?> GetByIdAsync(int id) =>
            await _context.CartItems
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.Category)
                .Include(ci => ci.Customer)
                .FirstOrDefaultAsync(ci => ci.Id == id);

        public async Task<CartItem> CreateAsync(CartItem item)
        {
            var customer = await _context.Customers.FindAsync(item.CustomerId);
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == item.ProductId);

            if (product == null || customer == null)
                throw new Exception("Product or Customer not found.");

            item.Product = product;
            item.Customer = customer;

            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(CartItem item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}

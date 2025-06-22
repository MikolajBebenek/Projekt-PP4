using FishingWorldEShop.Models;
using FishingWorldEShop.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FishingWorldEShop.Services
{
    public class ProductService : IProductService
    {
        private readonly FishingWorldEShopContext _context;

        public ProductService(FishingWorldEShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            if (product.Category == null && product.CategoryId != 0)
            {
                product.Category = await _context.Categories.FindAsync(product.CategoryId);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}

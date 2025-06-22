using FishingWorldEShop.Models;
using FishingWorldEShop.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FishingWorldEShop.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly FishingWorldEShopContext _context;

        public CustomerService(FishingWorldEShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();
        public async Task<Customer?> GetByIdAsync(int id) => await _context.Customers.FindAsync(id);
    }
}

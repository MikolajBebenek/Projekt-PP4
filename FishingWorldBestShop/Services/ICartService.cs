using FishingWorldEShop.Models;

namespace FishingWorldEShop.Services
{
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetAllAsync();
        Task<CartItem?> GetByIdAsync(int id);
        Task<CartItem> CreateAsync(CartItem item);
        Task UpdateAsync(CartItem item);
        Task DeleteAsync(int id);
    }
}

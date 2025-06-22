using FishingWorldEShop.Models;

namespace FishingWorldEShop.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);

    }
}

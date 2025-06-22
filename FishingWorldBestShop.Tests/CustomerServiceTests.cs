using Xunit;
using Microsoft.EntityFrameworkCore;
using FishingWorldEShop.Repositories;
using FishingWorldEShop.Services;
using FishingWorldEShop.Models;
using System.Threading.Tasks;

namespace FishingWorldEShop.Tests
{
    public class CustomerServiceTests
    {
        private FishingWorldEShopContext GetContext()
        {
            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase("TestDb_Customer")
                .Options;

            var context = new FishingWorldEShopContext(options);
            context.Customers.Add(new Customer
            {
                Id = 1,
                Name = "Jan",
                Surname = "Wêdkarz",
                Email = "jan@wedkarz.pl",
                Phone = "123456789",
                Address = "ul. Rybacka 5"
            });
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Customer()
        {
            var context = GetContext();
            var service = new CustomerService(context);

            var customer = await service.GetByIdAsync(1);

            Assert.NotNull(customer);
            Assert.Equal("Jan", customer!.Name);
        }
    }
}

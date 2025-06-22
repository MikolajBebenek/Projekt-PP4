using Xunit;
using Microsoft.EntityFrameworkCore;
using FishingWorldEShop.Repositories;
using FishingWorldEShop.Models;
using FishingWorldEShop;
using System.Threading.Tasks;

namespace FishingWorldEShop.Tests
{
    public class CartServiceTests
    {
        [Fact]
        public async Task CreateAsync_Adds_CartItem()
        {
            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase("CreateCartDb")
                .Options;

            using (var context = new FishingWorldEShopContext(options))
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Name = "Test",
                    Surname = "User",
                    Email = "test@example.com",
                    Phone = "123456789",
                    Address = "Testowa 1"
                });

                context.Categories.Add(new Category
                {
                    Id = 1,
                    Name = "TestCategory"
                });

                context.Products.Add(new Product
                {
                    Id = 1,
                    Name = "TestProduct",
                    Description = "Opis",
                    Price = 99,
                    EAN = 123456,
                    CategoryId = 1
                });

                context.SaveChanges();

                var service = new CartService(context);

                var item = new CartItem { CustomerId = 1, ProductId = 1, Quantity = 2 };
                var result = await service.CreateAsync(item);

                Assert.NotNull(result);
                Assert.Equal(2, result.Quantity);
                Assert.Equal(1, result.ProductId);
                Assert.Equal(1, result.CustomerId);
            }
        }


        [Fact]
        public async Task UpdateAsync_Changes_CartItem()
        {
            var dbName = "UpdateCartDb";

            var setupOptions = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using (var setupContext = new FishingWorldEShopContext(setupOptions))
            {
                setupContext.CartItems.Add(new CartItem
                {
                    Id = 1,
                    CustomerId = 1,
                    ProductId = 1,
                    Quantity = 1
                });
                setupContext.SaveChanges();
            }

            var updateOptions = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using (var updateContext = new FishingWorldEShopContext(updateOptions))
            {
                var service = new CartService(updateContext);
                var updated = new CartItem
                {
                    Id = 1,
                    CustomerId = 1,
                    ProductId = 1,
                    Quantity = 5
                };

                await service.UpdateAsync(updated);

                var result = await updateContext.CartItems.FindAsync(1);
                Assert.Equal(5, result!.Quantity);
            }
        }


        [Fact]
        public async Task DeleteAsync_Removes_CartItem()
        {
            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase("DeleteCartDb")
                .Options;

            var context = new FishingWorldEShopContext(options);
            context.CartItems.Add(new CartItem { Id = 1, CustomerId = 1, ProductId = 1, Quantity = 3 });
            context.SaveChanges();

            var service = new CartService(context);
            await service.DeleteAsync(1);

            var result = await context.CartItems.FindAsync(1);
            Assert.Null(result);
        }
    }
}

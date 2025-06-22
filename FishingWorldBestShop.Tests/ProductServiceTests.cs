using Xunit;
using Microsoft.EntityFrameworkCore;
using FishingWorldEShop.Repositories;
using FishingWorldEShop.Services;
using FishingWorldEShop.Models;
using System.Threading.Tasks;

namespace FishingWorldEShop.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task CreateAsync_Adds_Product()
        {
            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase("CreateProductDb")
                .Options;

            var context = new FishingWorldEShopContext(options);
            context.Categories.Add(new Category { Id = 1, Name = "Test" });
            context.SaveChanges();

            var service = new ProductService(context);
            var product = new Product
            {
                Name = "Nowa Wêdka",
                Description = "Do testowania",
                Price = 199.99m,
                CategoryId = 1
            };

            var result = await service.CreateAsync(product);

            Assert.NotNull(result);
            Assert.Equal("Nowa Wêdka", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_Changes_Product()
        {
            var dbName = "UpdateProductDb";

            var setupOptions = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using (var setupContext = new FishingWorldEShopContext(setupOptions))
            {
                setupContext.Categories.Add(new Category { Id = 1, Name = "Test" });
                setupContext.Products.Add(new Product
                {
                    Id = 1,
                    Name = "Stara Wêdka",
                    Description = "Opis",
                    Price = 100,
                    CategoryId = 1
                });
                setupContext.SaveChanges();
            }

            var updateOptions = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using (var updateContext = new FishingWorldEShopContext(updateOptions))
            {
                var service = new ProductService(updateContext);
                var updated = new Product
                {
                    Id = 1,
                    Name = "Zmieniona Wêdka",
                    Description = "Nowy opis",
                    Price = 150,
                    CategoryId = 1
                };

                await service.UpdateAsync(updated);

                var result = await updateContext.Products.FindAsync(1);
                Assert.Equal("Zmieniona Wêdka", result!.Name);
            }
        }



        [Fact]
        public async Task DeleteAsync_Removes_Product()
        {
            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase("DeleteProductDb")
                .Options;

            var context = new FishingWorldEShopContext(options);
            context.Products.Add(new Product
            {
                Id = 1,
                Name = "Do usuniêcia",
                Description = "Opis",
                Price = 99,
                CategoryId = 1
            });
            context.SaveChanges();

            var service = new ProductService(context);
            await service.DeleteAsync(1);

            var result = await context.Products.FindAsync(1);
            Assert.Null(result);
        }
    }
}

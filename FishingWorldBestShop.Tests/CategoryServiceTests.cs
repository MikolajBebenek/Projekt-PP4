using Xunit;
using Microsoft.EntityFrameworkCore;
using FishingWorldEShop.Repositories;
using FishingWorldEShop.Services;
using FishingWorldEShop.Models;
using System.Threading.Tasks;

namespace FishingWorldEShop.Tests
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task CreateAsync_Adds_Category()
        {
            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase("CreateCategoryDb")
                .Options;

            var context = new FishingWorldEShopContext(options);
            var service = new CategoryService(context);

            var category = new Category { Name = "Nowa Kategoria" };
            var result = await service.CreateAsync(category);

            Assert.NotNull(result);
            Assert.Equal("Nowa Kategoria", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_Changes_Category()
        {
            var dbName = "UpdateCategoryDb";

            var setupOptions = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using (var setupContext = new FishingWorldEShopContext(setupOptions))
            {
                setupContext.Categories.Add(new Category { Id = 1, Name = "Stara" });
                setupContext.SaveChanges();
            }

            var updateOptions = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using (var updateContext = new FishingWorldEShopContext(updateOptions))
            {
                var service = new CategoryService(updateContext);
                var updated = new Category { Id = 1, Name = "Nowa" };

                await service.UpdateAsync(updated);

                var result = await updateContext.Categories.FindAsync(1);
                Assert.Equal("Nowa", result!.Name);
            }
        }


        [Fact]
        public async Task DeleteAsync_Removes_Category()
        {
            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase("DeleteCategoryDb")
                .Options;

            var context = new FishingWorldEShopContext(options);
            context.Categories.Add(new Category { Id = 1, Name = "Do usuniêcia" });
            context.SaveChanges();

            var service = new CategoryService(context);
            await service.DeleteAsync(1);

            var result = await context.Categories.FindAsync(1);
            Assert.Null(result);
        }
    }
}

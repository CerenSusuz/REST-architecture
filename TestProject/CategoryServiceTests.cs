using FluentAssertions;
using Catalog.Database.DbContexts;
using Catalog.Services.Implementations;
using Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace TestProject
{
    public class CategoryServiceTests
    {
        private CatalogDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new CatalogDbContext(options);
        }

        [Fact]
        public async Task CreateCategory_Should_Add_Category()
        {
            // Arrange
            var context = GetDbContext();
            var service = new CategoryService(context);

            var category = new Category { Name = "Test Category", Description = "Description" };

            // Act
            var result = await service.CreateCategoryAsync(category);

            // Assert
            var categoriesInDb = await context.Categories.ToListAsync();
            categoriesInDb.Should().ContainSingle(c => c.Name == "Test Category");
        }

        [Fact]
        public async Task DeleteCategory_Should_Remove_Category()
        {
            // Arrange
            var context = GetDbContext();
            var service = new CategoryService(context);

            var category = new Category { Name = "To Be Deleted" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            // Act
            await service.DeleteCategoryAsync(category.Id);

            // Assert
            var categoriesInDb = await context.Categories.ToListAsync();
            categoriesInDb.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCategories_Should_Return_Categories()
        {
            // Arrange
            var context = GetDbContext();
            var service = new CategoryService(context);

            context.Categories.Add(new Category { Name = "Cat1" });
            context.Categories.Add(new Category { Name = "Cat2" });
            await context.SaveChangesAsync();

            // Act
            var categories = await service.GetCategoriesAsync();

            // Assert
            categories.Should().HaveCount(2);
        }
    }

}
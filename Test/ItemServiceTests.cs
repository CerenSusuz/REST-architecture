using Catalog.Database.DbContexts;
using Catalog.Models;
using Catalog.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace Test;

public class ItemServiceTests
{
    private CatalogDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CatalogDbContext(options);
    }

    [Fact]
    public async Task CreateItem_Should_Add_Item()
    {
        // Arrange
        var context = GetDbContext();
        var service = new ItemService(context);

        var category = new Category { Name = "Category1" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var item = new Item { Name = "Test Item", Price = 10m, CategoryId = category.Id };

        // Act
        var result = await service.CreateItemAsync(item);

        // Assert
        var itemsInDb = await context.Items.ToListAsync();
        itemsInDb.Should().ContainSingle(i => i.Name == "Test Item");
    }

    [Fact]
    public async Task DeleteItem_Should_Remove_Item()
    {
        // Arrange
        var context = GetDbContext();
        var service = new ItemService(context);

        var category = new Category { Name = "Category1" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var item = new Item { Name = "To Be Deleted", Price = 5m, CategoryId = category.Id };
        context.Items.Add(item);
        await context.SaveChangesAsync();

        // Act
        await service.DeleteItemAsync(item.Id);

        // Assert
        var itemsInDb = await context.Items.ToListAsync();
        itemsInDb.Should().BeEmpty();
    }

    [Fact]
    public async Task GetItems_Should_Return_Items()
    {
        // Arrange
        var context = GetDbContext();
        var service = new ItemService(context);

        var category = new Category { Name = "Category1" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        context.Items.Add(new Item { Name = "Item1", Price = 10m, CategoryId = category.Id });
        context.Items.Add(new Item { Name = "Item2", Price = 20m, CategoryId = category.Id });
        await context.SaveChangesAsync();

        // Act
        var items = await service.GetItemsAsync(null, 1, 10);

        // Assert
        items.Should().HaveCount(2);
    }
}

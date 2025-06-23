using Catalog.Models;

namespace Catalog.Services.Interfaces;

public interface IItemService
{
    Task<IEnumerable<Item>> GetItemsAsync(int? categoryId, int page, int pageSize);

    Task<Item> CreateItemAsync(Item item);

    Task UpdateItemAsync(Item item);

    Task DeleteItemAsync(int itemId);
}

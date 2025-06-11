using Microsoft.AspNetCore.Mvc;
using Catalog.Models;
using Catalog.Services.Interfaces;

namespace Catalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems([FromQuery] int? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var items = await _itemService.GetItemsAsync(categoryId, page, pageSize);

        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<Item>> CreateItem(Item item)
    {
        var created = await _itemService.CreateItemAsync(item);

        return CreatedAtAction(nameof(GetItems), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, Item item)
    {
        if (id != item.Id)
            return BadRequest();

        await _itemService.UpdateItemAsync(item);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        await _itemService.DeleteItemAsync(id);

        return NoContent();
    }
}
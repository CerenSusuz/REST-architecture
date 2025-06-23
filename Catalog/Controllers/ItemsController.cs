using Microsoft.AspNetCore.Mvc;
using Catalog.Models;
using Catalog.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Returns a paginated list of items, optionally filtered by category.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Get items",
            Description = "Retrieves a list of items. Supports optional filtering by category and pagination."
        )]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems([FromQuery] int? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var items = await _itemService.GetItemsAsync(categoryId, page, pageSize);

            return Ok(items);
        }

        /// <summary>
        /// Creates a new item.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Item), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Create item",
            Description = "Creates a new item under a given category."
        )]
        public async Task<ActionResult<Item>> CreateItem(Item item)
        {
            var created = await _itemService.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItems), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing item by ID.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Update item",
            Description = "Updates an item’s details by its ID."
        )]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            if (id != item.Id)
                return BadRequest();

            await _itemService.UpdateItemAsync(item);

            return NoContent();
        }

        /// <summary>
        /// Deletes an item by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Delete item",
            Description = "Deletes a specific item by its ID."
        )]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _itemService.DeleteItemAsync(id);

            return NoContent();
        }
    }
}

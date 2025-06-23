using Microsoft.AspNetCore.Mvc;
using Catalog.Models;
using Catalog.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Returns a list of all categories.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Get all categories",
            Description = "Retrieves a list of all available categories."
        )]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();

            return Ok(categories);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Create category",
            Description = "Creates a new category and returns it."
        )]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            var created = await _categoryService.CreateCategoryAsync(category);

            return CreatedAtAction(nameof(GetCategories), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing category by ID.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Update category",
            Description = "Updates the details of a category by its ID."
        )]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.Id)
                return BadRequest();

            await _categoryService.UpdateCategoryAsync(category);

            return NoContent();
        }

        /// <summary>
        /// Deletes a category and its related items.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Delete category",
            Description = "Deletes a category by its ID, along with related items."
        )]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);

            return NoContent();
        }
    }
}

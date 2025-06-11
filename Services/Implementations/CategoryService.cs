using Catalog.Database.DbContexts;
using Catalog.Models;
using Catalog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly CatalogDbContext _context;

    public CategoryService(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.Include(c => c.Items).ToListAsync();
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Entry(category).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int categoryId)
    {
        var category = await _context.Categories.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == categoryId);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}

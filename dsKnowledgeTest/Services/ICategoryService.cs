using dsKnowledgeTest.Data;
using dsKnowledgeTest.Models;
using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<string?> GetNameOfCategoryAsync(Guid categoryId);
    Task<Category?> GetCategoryByIdAsync(Guid categoryId);
    Task<List<string?>> GetNamesOfCategoriesAsync();
}

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync() =>
        await _db.Categories.ToListAsync();

    public async Task<string?> GetNameOfCategoryAsync(Guid categoryId) =>
        (await _db.Categories.FirstOrDefaultAsync(category => category.Id == categoryId))?.Name;

    public async Task<Category?> GetCategoryByIdAsync(Guid categoryId) =>
        await _db.Categories.FirstOrDefaultAsync(category => category.Id == categoryId);

    public async Task<List<string?>> GetNamesOfCategoriesAsync() =>
        await _db.Categories.Select(category => category.Name).ToListAsync();
}
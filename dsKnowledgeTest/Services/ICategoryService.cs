using dsKnowledgeTest.Data;
using dsKnowledgeTest.Models;
using dsKnowledgeTest.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<string?> GetNameOfCategoryAsync(Guid categoryId);
    Task<Category?> GetCategoryByIdAsync(Guid categoryId);
    Task<List<string?>> GetNamesOfCategoriesAsync();
    Task CreateCategoryAsync(CategoryViewModel category);
    Task EditCategoryAsync(CategoryViewModel category);
    Task DeleteCategoryAsync(Guid categoryId);
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

    public async Task CreateCategoryAsync(CategoryViewModel category)
    {
        await _db.Categories.AddAsync(new Category()
        {
            Name = category.Name,
            Description = category.Description,
            CntTest = category.CntTest,
            CreatedDate = new DateTime(),
            ImageUrl = category.ImageUrl,
            UpdatedDate = new DateTime(),
        });
        await _db.SaveChangesAsync();
    }

    public async Task EditCategoryAsync(CategoryViewModel category)
    {
        var categoryVm = await _db.Categories.FirstOrDefaultAsync(categoryItem => categoryItem.Id == category.Id);
        if (categoryVm != null)
        {
            _db.Categories.Update(new Category()
            {
                Id = categoryVm.Id,
                Description = category.Description ?? categoryVm.Description,
                Name = category.Name ?? categoryVm.Name,
                CntTest = category.CntTest ?? categoryVm.CntTest,
                ImageUrl = category.ImageUrl ?? categoryVm.ImageUrl,
                CreatedDate = categoryVm.CreatedDate,
                UpdatedDate = new DateTime(),
            });
            await _db.SaveChangesAsync();
        }
    }

    public async Task DeleteCategoryAsync(Guid categoryId)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(categoryItem => categoryItem.Id == categoryId);
        if (category != null)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }
    }
}
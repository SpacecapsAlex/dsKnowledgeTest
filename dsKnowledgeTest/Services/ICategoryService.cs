using dsKnowledgeTest.Data;
using dsKnowledgeTest.Models;
using dsKnowledgeTest.ViewModels.CategoryViewModels;
using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<string?> GetNameOfCategoryAsync(Guid categoryId);
    Task<CategoryViewModel?> GetCategoryByIdAsync(Guid categoryId);
    Task<List<string?>> GetNamesOfCategoriesAsync();
    Task CreateCategoryAsync(CreateCategoryViewModel category);
    Task EditCategoryAsync(EditCategoryViewModel category);
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
        await _db.Categories
            .Include("Tests")
            .Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                CntTest = c.Tests.Count(t => t.IsDeleted == false),
                IsDeleted = c.IsDeleted,
                CreatedDate = c.CreatedDate,
                UpdatedDate = c.UpdatedDate,
                Tests = null
            })
            .Where(c => c.IsDeleted == false)
            .ToListAsync();

    public async Task<string?> GetNameOfCategoryAsync(Guid categoryId) =>
        (await _db.Categories.FirstOrDefaultAsync(category => category.Id == categoryId))?.Name;

    public async Task<CategoryViewModel?> GetCategoryByIdAsync(Guid categoryId)
    {
        return await _db.Categories
            .Include("Tests")
            .Select(c => new CategoryViewModel
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Description = c.Description,
                CntTest = c.Tests.Count(t => t.IsDeleted == false),
                ImageUrl = c.ImageUrl
            })
            .FirstOrDefaultAsync(category => category.Id == categoryId.ToString());
    }

    public async Task<List<string?>> GetNamesOfCategoriesAsync() =>
        await _db.Categories.Select(category => category.Name).ToListAsync();

    public async Task CreateCategoryAsync(CreateCategoryViewModel category)
    {
        await _db.Categories.AddAsync(new Category()
        {
            Name = category.Name,
            Description = category.Description,
            CntTest = 0,
            CreatedDate = DateTime.Now,
            ImageUrl = category.ImageUrl,
            UpdatedDate = DateTime.Now,
            IsDeleted = false
        });
        await _db.SaveChangesAsync();
    }

    public async Task EditCategoryAsync(EditCategoryViewModel category)
    {
        var categoryVm =
            await _db.Categories.FirstOrDefaultAsync(categoryItem => categoryItem.Id.ToString() == category.Id);
        if (categoryVm != null)
        {
            categoryVm.Description = category.Description ?? categoryVm.Description;
            categoryVm.Name = category.Name ?? categoryVm.Name;
            categoryVm.ImageUrl = category.ImageUrl ?? categoryVm.ImageUrl;
            categoryVm.UpdatedDate = DateTime.Now;

            _db.Categories.Update(categoryVm);
            await _db.SaveChangesAsync();
        }
    }

    public async Task DeleteCategoryAsync(Guid categoryId)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(categoryItem => categoryItem.Id == categoryId);
        if (category != null)
        {
            category.IsDeleted = true;
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
        }
    }
}
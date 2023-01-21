using dsKnowledgeTest.Data;
using dsKnowledgeTest.Models;
using dsKnowledgeTest.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Services;

public interface ITestService
{
    Task<List<Test>> GetAllTestsAsync();
    Task<List<Test>> GetAllTestByCategoryAsync(Guid categoryId);
    Task<Test?> GetTestByIdAsync(Guid testId);
    Task CreateTestAsync(TestViewModel test);
    Task EditTestAsync(TestViewModel test);
    Task DeleteTestAsync(Guid testId);
}

public class TestService : ITestService
{
    private readonly AppDbContext _db;

    public TestService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Test>> GetAllTestsAsync() =>
        await _db.Tests.ToListAsync();

    public async Task<List<Test>> GetAllTestByCategoryAsync(Guid categoryId) =>
        await _db.Tests.Where(test => test.CategoryId == categoryId).ToListAsync();

    public async Task<Test?> GetTestByIdAsync(Guid testId) =>
        await _db.Tests.FirstOrDefaultAsync(test => test.Id == testId);

    public async Task CreateTestAsync(TestViewModel test)
    {
        await _db.Tests.AddAsync(new Test()
        {
            TestLevel = test.TestLevel,
            CategoryId = test.CategoryId,
            Description = test.Description,
            Name = test.Name,
            CntQuestion = test.CntQuestion,
            CreatedDate = new DateTime(),
            ImageUrl = test.ImageUrl,
            UpdatedDate = new DateTime(),
            TimeForTest = test.TimeForTest,
            IsTestOnTime = test.IsTestOnTime,
        });
        await _db.SaveChangesAsync();
    }

    public async Task EditTestAsync(TestViewModel test)
    {
        var testVm = await _db.Tests.FirstOrDefaultAsync(testItem => testItem.Id == test.Id);
        if (testVm != null)
        {
            _db.Tests.Update(new Test()
            {
                Id = testVm.Id,
                CategoryId = test.CategoryId,
                Description = test.Description ?? testVm.Description,
                CntQuestion = test.CntQuestion ?? testVm.CntQuestion,
                CreatedDate = testVm.CreatedDate,
                UpdatedDate = new DateTime(),
                ImageUrl = test.ImageUrl ?? testVm.ImageUrl,
                TimeForTest = test.TimeForTest ?? testVm.TimeForTest,
                IsTestOnTime = test.IsTestOnTime ?? testVm.IsTestOnTime,
                TestLevel = test.TestLevel ?? testVm.TestLevel,
                Name = test.Name ?? testVm.Name,
            });
            await _db.SaveChangesAsync();
        }
    }

    public async Task DeleteTestAsync(Guid testId)
    {
        var test = await _db.Tests.FirstOrDefaultAsync(testItem => testItem.Id == testId);
        if (test != null)
        {
            _db.Tests.Remove(test);
            await _db.SaveChangesAsync();
        }
    }
}
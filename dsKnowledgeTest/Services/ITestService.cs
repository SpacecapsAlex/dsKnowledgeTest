using dsKnowledgeTest.Data;
using dsKnowledgeTest.Models;
using dsKnowledgeTest.ViewModels.QuestionViewModels;
using dsKnowledgeTest.ViewModels.TestViewModel;
using dsKnowledgeTest.ViewModels.TestViewModels;
using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Services;

public interface ITestService
{
    Task<List<Test>> GetAllTestsAsync();
    Task<List<Test>> GetAllTestByCategoryAsync(Guid categoryId);
    Task<TestViewModel?> GetTestByIdAsync(Guid testId);
    Task CreateTestAsync(CreateTestViewModel test);
    Task EditTestAsync(EditTestViewModel test);
    Task DeleteTestAsync(Guid testId);
    Task CreateTestWithQuestionsAsync(CreateTestWithQuestionsViewModel test);
    Task EditTestWithQuestionsAsync(EditTestWithQuestionsViewModel test);
}

public class TestService : ITestService
{
    private readonly AppDbContext _db;
    private readonly IQuestionService _questionService;

    public TestService(AppDbContext db, IQuestionService questionService)
    {
        _db = db;
        _questionService = questionService;
    }

    public async Task<List<Test>> GetAllTestsAsync() =>
        await _db.Tests
            .Where(t => t.IsDeleted == false)
            .ToListAsync();

    public async Task<List<Test>> GetAllTestByCategoryAsync(Guid categoryId) =>
        await _db.Tests.Where(test => test.CategoryId == categoryId).ToListAsync();

    public  async Task<TestViewModel?> GetTestByIdAsync(Guid testId) {
        return await _db.Tests.Select(t => new TestViewModel
        {
            Id = t.Id.ToString(),
            Name = t.Name,
            Description = t.Description,
            ImageUrl = t.ImageUrl,
            TestLevel = t.TestLevel,
            IsTestOnTime = t.IsTestOnTime,
            TimeForTest = t.TimeForTest,
            Score = t.Score,
            CntQuestion = t.CntQuestion,
            CategoryId = t.CategoryId.ToString()
        }).FirstOrDefaultAsync(x => x.Id == testId.ToString());
    }

    public async Task CreateTestAsync(CreateTestViewModel test)
    {
        await _db.Tests.AddAsync(new Test()
        {
            TestLevel = test.TestLevel,
            CategoryId = Guid.Parse(test.CategoryId),
            Description = test.Description,
            Name = test.Name,
            CntQuestion = 0,
            CreatedDate = new DateTime(),
            ImageUrl = test.ImageUrl,
            UpdatedDate = new DateTime(),
            TimeForTest = test.TimeForTest,
            IsTestOnTime = test.IsTestOnTime,
            Score = test.Score,
            IsDeleted = false
        });
        await _db.SaveChangesAsync();

        var categoryTest = await _db.Categories.FirstOrDefaultAsync(x => x.Id.ToString() == test.CategoryId);
        if (categoryTest != null)
        {
            categoryTest.CntTest++;
        }
        await _db.SaveChangesAsync();
    }

    public async Task EditTestAsync(EditTestViewModel test)
    {
        var testVm = await _db.Tests.FirstOrDefaultAsync(testItem => testItem.Id.ToString() == test.Id);
        if (testVm != null)
        {
            testVm.Name = test.Name ?? testVm.Name;
            testVm.Description = test.Description ?? testVm.Description;
            testVm.UpdatedDate = new DateTime();
            testVm.TestLevel = test.TestLevel ?? testVm.TestLevel;
            testVm.ImageUrl = test.ImageUrl ?? testVm.ImageUrl;
            testVm.TimeForTest = test.TimeForTest ?? testVm.TimeForTest;
            testVm.IsTestOnTime = test.IsTestOnTime ?? testVm.IsTestOnTime;
            testVm.TestLevel = test.TestLevel ?? testVm.TestLevel;
            testVm.Score = test.Score ?? testVm.Score;

            _db.Tests.Update(testVm);
            await _db.SaveChangesAsync();
        }
    }

    public async Task DeleteTestAsync(Guid testId)
    {
        var test = await _db.Tests.FirstOrDefaultAsync(testItem => testItem.Id == testId);
        if (test != null)
        {
            test.IsDeleted = true;
            _db.Tests.Update(test);

            var categoryTest = await _db.Categories.FirstOrDefaultAsync(x => x.Id == test.CategoryId);
            if (categoryTest != null)
            {
                categoryTest.CntTest--;
            }
            await _db.SaveChangesAsync();
        }
    }

    public async Task CreateTestWithQuestionsAsync(CreateTestWithQuestionsViewModel test)
    {
        await CreateTestAsync(new CreateTestViewModel
        {
            Name = test.Name,
            Description = test.Description,
            CategoryId = test.CategoryId,
            ImageUrl = test.ImageUrl,
            IsTestOnTime = test.IsTestOnTime,
            Score = test.Score,
            TestLevel = test.TestLevel,
            TimeForTest = test.TimeForTest
        });
        await _db.SaveChangesAsync();

        var questionId = (await _db.Tests.FirstOrDefaultAsync(t => t.Name == test.Name)).Id;

        foreach (var q in test.Questions)
        {
            await _questionService.CreateQuestionAsync(new CreateQuestionViewModel
            {
                Name = q.Name,
                QuestionType = q.QuestionType,
                NumberOfPoints = q.NumberOfPoints,
                IconUrl = q.IconUrl,
                TestId = questionId.ToString(),
                Answers = q.Answers,
                TrueAnswers = q.TrueAnswers,
            });
        }
    }

    public async Task EditTestWithQuestionsAsync(EditTestWithQuestionsViewModel test)
    {
        await EditTestAsync(new EditTestViewModel
        {
            Id = test.Id,
            Name = test.Name,
            Description = test.Description,
            CategoryId = test.CategoryId,
            ImageUrl = test.ImageUrl,
            IsTestOnTime = test.IsTestOnTime,
            Score = test.Score,
            TestLevel = test.TestLevel,
            TimeForTest = test.TimeForTest
        });
        await _db.SaveChangesAsync();

        var questionId = (await _db.Tests.FirstOrDefaultAsync(t => t.Name == test.Name)).Id;

        foreach (var q in test.Questions)
        {
            await _questionService.EditQuestionAsync(new EditQuestionViewModel
            {
                Id = q.Id,
                Name = q.Name,
                QuestionType = q.QuestionType,
                NumberOfPoints = q.NumberOfPoints,
                IconUrl = q.IconUrl,
                TestId = questionId.ToString(),
                Answers = q.Answers,
                TrueAnswers = q.TrueAnswers,
            });
        }
    }
}
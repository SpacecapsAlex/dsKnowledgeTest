using dsKnowledgeTest.Constants;
using dsKnowledgeTest.Data;
using dsKnowledgeTest.Models;
using dsKnowledgeTest.ViewModels.QuestionViewModels;
using dsKnowledgeTest.ViewModels.TestViewModel;
using dsKnowledgeTest.ViewModels.TestViewModels;
using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Services;

public interface ITestService
{
    Task<List<TestViewModel>> GetAllTestsAsync();
    Task<List<TestViewModel>> GetAllTestByCategoryAsync(Guid categoryId);
    Task<TestViewModel?> GetTestByIdAsync(Guid testId);
    Task<TestWithQuestionsViewModel?> GetTestByIdWithQuestionsAsync(Guid testId);
    Task CreateTestAsync(CreateTestViewModel test);
    Task EditTestAsync(EditTestViewModel test);
    Task DeleteTestAsync(Guid testId);
    Task CreateTestWithQuestionsAsync(CreateTestWithQuestionsViewModel test);
    Task EditTestWithQuestionsAsync(EditTestWithQuestionsViewModel test);
    Task<List<TestViewModel>> SearchTestsAsync(string testName);
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

    public async Task<List<TestViewModel>> GetAllTestsAsync() =>
        await _db.Tests
            .Where(t => t.IsDeleted == false)
            .Include("Questions")
            .Select(t => new TestViewModel
            {
                Id = t.Id.ToString(),
                Name = t.Name,
                Description = t.Description,
                ImageUrl = t.ImageUrl,
                TestLevel = t.TestLevel.ToString(),
                TimeForTest = t.TimeForTest,
                Score = t.Score,
                CntQuestion = t.Questions == null ? 0 : t.Questions.Count(q => q.IsDeleted == false),
                CategoryId = t.CategoryId.ToString(),
                IsRandomAnswers = t.IsRandomAnswers,
                IsRandomQuestions = t.IsRandomQuestions
            })
            .ToListAsync();

    public async Task<List<TestViewModel>> GetAllTestByCategoryAsync(Guid categoryId) =>
        await _db.Tests
            .Where(test => test.CategoryId == categoryId && test.IsDeleted == false)
            .Include("Questions")
            .Select(t => new TestViewModel
            {
                Id = t.Id.ToString(),
                Name = t.Name,
                Description = t.Description,
                ImageUrl = t.ImageUrl,
                TestLevel = t.TestLevel.ToString(),
                TimeForTest = t.TimeForTest,
                Score = t.Score,
                CntQuestion = t.Questions == null ? 0 : t.Questions.Count(q => q.IsDeleted == false),
                CategoryId = t.CategoryId.ToString(),
                IsRandomAnswers = t.IsRandomAnswers,
                IsRandomQuestions = t.IsRandomQuestions
            }).ToListAsync();

    public async Task<TestViewModel?> GetTestByIdAsync(Guid testId)
    {
        return await _db.Tests
            .Include("Questions")
            .Where(test => test.IsDeleted == false)
            .Select(t => new TestViewModel
            {
                Id = t.Id.ToString(),
                Name = t.Name,
                Description = t.Description,
                ImageUrl = t.ImageUrl,
                TestLevel = t.TestLevel.ToString(),
                IsTestOnTime = t.IsTestOnTime,
                TimeForTest = t.TimeForTest,
                Score = t.Score,
                CntQuestion = t.Questions == null ? 0 : t.Questions.Count(q => q.IsDeleted == false),
                CategoryId = t.CategoryId.ToString(),
                IsRandomAnswers = t.IsRandomAnswers,
                IsRandomQuestions = t.IsRandomQuestions
            }).FirstOrDefaultAsync(x => x.Id == testId.ToString());
    }

    public async Task<TestWithQuestionsViewModel?> GetTestByIdWithQuestionsAsync(Guid testId)
    {
        var tests = await _db.Tests
            .Include("Questions")
            .Where(test => test.IsDeleted == false)
            .Select(t => new TestWithQuestionsViewModel
            {
                Id = t.Id.ToString(),
                Name = t.Name,
                Description = t.Description,
                ImageUrl = t.ImageUrl,
                TestLevel = t.TestLevel.ToString(),
                IsTestOnTime = t.IsTestOnTime,
                TimeForTest = t.TimeForTest,
                Score = t.Score,
                CntQuestion = t.Questions == null ? 0 : t.Questions.Count(q => q.IsDeleted == false),
                CategoryId = t.CategoryId.ToString(),
                IsRandomAnswers = t.IsRandomAnswers,
                IsRandomQuestions = t.IsRandomQuestions,
                Questions = new List<QuestionViewModel>()
            }).FirstOrDefaultAsync(x => x.Id == testId.ToString());
        var questions =
            await _questionService.GetAllQuestionForTestWithSortAsync(testId, tests.IsRandomQuestions,
                tests.IsRandomAnswers);
        tests.Questions = questions;
        return tests;
    }

    public async Task CreateTestAsync(CreateTestViewModel test)
    {
        await _db.Tests.AddAsync(new Test()
        {
            TestLevel = (TestLevel)Enum.Parse(typeof(TestLevel), test.TestLevel),
            CategoryId = Guid.Parse(test.CategoryId),
            Description = test.Description,
            Name = test.Name,
            CntQuestion = 0,
            CreatedDate = DateTime.Now,
            ImageUrl = test.ImageUrl,
            UpdatedDate = DateTime.Now,
            TimeForTest = test.TimeForTest,
            IsTestOnTime = test.IsTestOnTime,
            Score = test.Score,
            IsDeleted = false,
            IsRandomAnswers = test.IsRandomAnswers,
            IsRandomQuestions = test.IsRandomQuestions
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
            testVm.UpdatedDate = DateTime.Now;
            testVm.ImageUrl = test.ImageUrl ?? testVm.ImageUrl;
            testVm.TimeForTest = test.TimeForTest ?? testVm.TimeForTest;
            testVm.IsTestOnTime = test.IsTestOnTime ?? testVm.IsTestOnTime;
            testVm.TestLevel = (TestLevel)Enum.Parse(typeof(TestLevel), test.TestLevel);
            testVm.Score = test.Score ?? testVm.Score;
            testVm.IsRandomQuestions = test.IsRandomQuestions ?? testVm.IsRandomQuestions;
            testVm.IsRandomAnswers = test.IsRandomAnswers ?? testVm.IsRandomAnswers;

            _db.Tests.Update(testVm);
            await _db.SaveChangesAsync();
        }
    }

    public async Task DeleteTestAsync(Guid testId)
    {
        var test = await _db.Tests
            .Include("Questions")
            .FirstOrDefaultAsync(testItem => testItem.Id == testId);
        if (test != null)
        {
            test.IsDeleted = true;
            _db.Tests.Update(test);
            await _db.SaveChangesAsync();

            foreach (var question in test.Questions)
            {
                question.IsDeleted = true;
                _db.Questions.Update(question);
                await _db.SaveChangesAsync();
            }

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
            TimeForTest = test.TimeForTest,
            IsRandomAnswers = test.IsRandomAnswers,
            IsRandomQuestions = test.IsRandomQuestions
        });
        await _db.SaveChangesAsync();

        var questionId = (await _db.Tests
            .FirstOrDefaultAsync(t => t.Name == test.Name &&
                                      t.Score == test.Score &&
                                      t.Description == test.Description && 
                                      t.CategoryId.ToString() == test.CategoryId)).Id;

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
            TimeForTest = test.TimeForTest,
            IsRandomAnswers = test.IsRandomAnswers,
            IsRandomQuestions = test.IsRandomQuestions
        });
        await _db.SaveChangesAsync();

        var questionInTest = await _questionService.GetAllQuestionForTestAsync(Guid.Parse(test.Id));

        if (questionInTest != null)
            foreach (var qtn in questionInTest)
            {
                var deleteQuestion =
                    test.Questions.Find(item => item.Id == qtn.Id);
                if (deleteQuestion == null)
                {
                    await _questionService.DeleteQuestionAsync(Guid.Parse(qtn.Id));
                }
            }

        foreach (var q in test.Questions)
        {
            if (q.Id == null)
            {
                await _questionService.CreateQuestionAsync(new CreateQuestionViewModel
                {
                    Name = q.Name,
                    QuestionType = q.QuestionType,
                    NumberOfPoints = q.NumberOfPoints ?? 0,
                    IconUrl = q.IconUrl,
                    TestId = test.Id,
                    Answers = q.Answers,
                    TrueAnswers = q.TrueAnswers,
                });
            }
            else
            {
                await _questionService.EditQuestionAsync(new EditQuestionViewModel
                {
                    Id = q.Id,
                    Name = q.Name,
                    QuestionType = q.QuestionType,
                    NumberOfPoints = q.NumberOfPoints,
                    IconUrl = q.IconUrl,
                    TestId = test.Id,
                    Answers = q.Answers,
                    TrueAnswers = q.TrueAnswers,
                });
            }
        }
        await _db.SaveChangesAsync();
    }

    public async Task<List<TestViewModel>> SearchTestsAsync(string testName)
    {
        return await _db.Tests
            .Include("Questions")
            .Where(t => t.IsDeleted == false &&
                        (t.Name.ToLower().Contains(testName.ToLower()) ||
                        t.Description.ToLower().Contains(testName.ToLower())))
            .Select(t => new TestViewModel
            {
                Id = t.Id.ToString(),
                Name = t.Name,
                Description = t.Description,
                ImageUrl = t.ImageUrl,
                TestLevel = t.TestLevel.ToString(),
                TimeForTest = t.TimeForTest,
                Score = t.Score,
                CntQuestion = t.Questions == null ? 0 : t.Questions.Count(q => q.IsDeleted == false),
                CategoryId = t.CategoryId.ToString(),
                IsRandomAnswers = t.IsRandomAnswers,
                IsRandomQuestions = t.IsRandomQuestions
            })
            .ToListAsync();
    }
}
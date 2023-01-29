using dsKnowledgeTest.Data;
using dsKnowledgeTest.Models;
using dsKnowledgeTest.ViewModels.QuestionViewModels;
using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Services;

public interface IQuestionService
{
    Task<List<QuestionViewModel>?> GetAllQuestionForTestAsync(Guid testId);
    Task<QuestionViewModel?> GetQuestionByIdAsync(Guid questionId);
    Task CreateQuestionAsync(CreateQuestionViewModel question);
    Task EditQuestionAsync(EditQuestionViewModel question);
    Task DeleteQuestionAsync(Guid questionId);
}

public class QuestionService : IQuestionService
{
    private readonly AppDbContext _db;

    public QuestionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<QuestionViewModel>?> GetAllQuestionForTestAsync(Guid testId) =>
        await _db.Questions
            .Select(q => new QuestionViewModel
            {
                Id = q.Id.ToString(),
                Name = q.Name,
                QuestionType = q.QuestionType,
                NumberOfPoints = q.NumberOfPoints,
                IconUrl = q.IconUrl,
                TestId = q.TestId.ToString(),
                Answers = q.ListAnswers,
                TrueAnswers = q.ListTrueAnswers
            }).Where(x => x.TestId == testId.ToString()).ToListAsync();

    public async Task<QuestionViewModel?> GetQuestionByIdAsync(Guid questionId)
    {
        return await _db.Questions
            .Select(q => new QuestionViewModel
            {
                Id = q.Id.ToString(),
                Name = q.Name,
                QuestionType = q.QuestionType,
                NumberOfPoints = q.NumberOfPoints,
                IconUrl = q.IconUrl,
                TestId = q.TestId.ToString(),
                Answers = q.ListAnswers,
                TrueAnswers = q.ListTrueAnswers
            })
            .FirstOrDefaultAsync(x => x.Id == questionId.ToString());
    }

    public async Task CreateQuestionAsync(CreateQuestionViewModel question)
    {
        await _db.Questions.AddAsync(new Question
        {
            Name = question.Name,
            QuestionType = question.QuestionType,
            NumberOfPoints = question.NumberOfPoints,
            IconUrl = question.IconUrl,
            TestId = Guid.Parse(question.TestId),
            ListAnswers = question.Answers,
            ListTrueAnswers = question.TrueAnswers
        });
        await _db.SaveChangesAsync();

        var testQuestion = await _db.Tests.FirstOrDefaultAsync(x => x.Id.ToString() == question.TestId);
        if (testQuestion != null)
        {
            testQuestion.CntQuestion++;
        }
        await _db.SaveChangesAsync();
    }

    public async Task EditQuestionAsync(EditQuestionViewModel question)
    {
        var questionVm = await _db.Questions.FirstOrDefaultAsync(q => q.Id.ToString() == question.Id);
        if (questionVm != null)
        {
            questionVm.Name = question.Name ?? questionVm.Name;
            questionVm.QuestionType = question.QuestionType ?? questionVm.QuestionType;
            questionVm.NumberOfPoints = question.NumberOfPoints ?? questionVm.NumberOfPoints;
            questionVm.IconUrl = question.IconUrl ?? questionVm.IconUrl;
            questionVm.TestId = Guid.Parse(question.TestId);
            questionVm.ListAnswers = question.Answers ?? questionVm.ListAnswers;
            questionVm.ListTrueAnswers = question.TrueAnswers ?? questionVm.ListTrueAnswers;
            
            _db.Questions.Update(questionVm);
            await _db.SaveChangesAsync();
        }
    }

    public async Task DeleteQuestionAsync(Guid questionId)
    {
        var question = await _db.Questions.FirstOrDefaultAsync(questionItem => questionItem.Id == questionId);
        if (question != null)
        {
            _db.Questions.Remove(question);

            var testQuestion = await _db.Tests.FirstOrDefaultAsync(x => x.Id == question.TestId);
            if (testQuestion != null)
            {
                testQuestion.CntQuestion--;
            }
            await _db.SaveChangesAsync();
        }
    }
}
using dsKnowledgeTest.Constants;
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
        await _db.Questions.Where(q => q.IsDeleted == false)
            .Select(q => new QuestionViewModel
            {
                Id = q.Id.ToString(),
                Name = q.Name,
                QuestionType = q.QuestionType.ToString(),
                NumberOfPoints = q.NumberOfPoints,
                IconUrl = q.IconUrl,
                Explanation = q.Explanation,
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
                QuestionType = q.QuestionType.ToString(),
                NumberOfPoints = q.NumberOfPoints,
                IconUrl = q.IconUrl,
                Explanation = q.Explanation,
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
            QuestionType = (QuestionType) Enum.Parse(typeof(QuestionType), question.QuestionType),
            NumberOfPoints = question.NumberOfPoints,
            IconUrl = question.IconUrl,
            Explanation = question.Explanation,
            TestId = Guid.Parse(question.TestId),
            ListAnswers = question.Answers,
            ListTrueAnswers = question.TrueAnswers,
            IsDeleted = false
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
            questionVm.QuestionType = (QuestionType) Enum.Parse(typeof(QuestionType), question.QuestionType);
            questionVm.NumberOfPoints = question.NumberOfPoints ?? questionVm.NumberOfPoints;
            questionVm.IconUrl = question.IconUrl ?? questionVm.IconUrl;
            questionVm.TestId = Guid.Parse(question.TestId);
            questionVm.Explanation = question.Explanation ?? questionVm.Explanation;
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
            question.IsDeleted = true;
            _db.Questions.Update(question);

            var testQuestion = await _db.Tests.FirstOrDefaultAsync(x => x.Id == question.TestId);
            if (testQuestion != null)
            {
                testQuestion.CntQuestion--;
            }
            await _db.SaveChangesAsync();
        }
    }

    private List<string> GetRandomSort(List<string> list)
    {
        Random random = new Random();
        for (int i = 0; i < list.Count; i++)
        {
            int j = random.Next(i + 1);

            (list[j], list[i]) = (list[i], list[j]);
        }
        return list;
    }
}
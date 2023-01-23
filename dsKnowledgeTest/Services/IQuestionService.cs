using dsKnowledgeTest.Data;
using dsKnowledgeTest.Models;
using Microsoft.EntityFrameworkCore;

namespace dsKnowledgeTest.Services;

public interface IQuestionService
{
    Task<List<Question>> GetAllQuestionForTestAsync(Guid testId);
}

public class QuestionService : IQuestionService
{
    private readonly AppDbContext _db;

    public QuestionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Question>> GetAllQuestionForTestAsync(Guid testId) =>
        await _db.Questions
            .Where(questionItem => questionItem.TestId == testId).ToListAsync();
}
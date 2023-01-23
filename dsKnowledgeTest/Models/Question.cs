using dsKnowledgeTest.Constants;

namespace dsKnowledgeTest.Models;

public class Question
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public QuestionType QuestionType { get; set; }
    public List<string>? Answers { get; set; }
    public List<string>? TrueAnswer { get; set; }
    public int NumberOfPoints { get; set; }
    public string? IconUrl { get; set; }

    public Guid? TestId { get; set; }
    public Test? Test { get; set; }
}
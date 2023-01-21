using dsKnowledgeTest.Constants;

namespace dsKnowledgeTest.ViewModels;

public class TestViewModel
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public TestLevel? TestLevel { get; set; }
    public bool? IsTestOnTime { get; set; }
    public TimeSpan? TimeForTest { get; set; }
    public int? CntQuestion { get; set; }
    public Guid CategoryId { get; set; }
}
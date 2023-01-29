using dsKnowledgeTest.Constants;

namespace dsKnowledgeTest.ViewModels.TestViewModel
{
    public class CreateTestViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public TestLevel? TestLevel { get; set; }
        public bool? IsTestOnTime { get; set; }
        public int? TimeForTest { get; set; }
        public int? Score { get; set; }
        public string CategoryId { get; set; }
    }
}

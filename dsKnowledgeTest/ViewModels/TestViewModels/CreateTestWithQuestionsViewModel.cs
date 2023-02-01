using dsKnowledgeTest.Constants;
using dsKnowledgeTest.ViewModels.QuestionViewModels;

namespace dsKnowledgeTest.ViewModels.TestViewModels
{
    public class CreateTestWithQuestionsViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? TestLevel { get; set; }
        public bool? IsTestOnTime { get; set; }
        public int? TimeForTest { get; set; }
        public int? Score { get; set; }
        public string CategoryId { get; set; }
        public List<CreateQuestionWithoutTestIdViewModel>? Questions{ get; set; }
}
}

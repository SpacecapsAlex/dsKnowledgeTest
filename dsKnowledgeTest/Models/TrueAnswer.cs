namespace dsKnowledgeTest.Models
{
    public class TrueAnswer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
    }
}

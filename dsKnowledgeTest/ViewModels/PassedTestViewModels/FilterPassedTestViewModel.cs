namespace dsKnowledgeTest.ViewModels.PassedTestViewModels
{
    public class FilterPassedTestViewModel
    {
        public string UserId { get; set; }
        public List<string?> Categoryes { get; set; }
        public List<string?> Statuses { get; set; }
        public int? MinScore { get; set; }
        public int? MaxScore { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
    }
}

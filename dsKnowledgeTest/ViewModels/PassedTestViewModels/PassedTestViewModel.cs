﻿using dsKnowledgeTest.ViewModels.AnsweredQuestionViewModels;

namespace dsKnowledgeTest.ViewModels.PassedTestViewModels
{
    public class PassedTestViewModel
    {
        public string Id { get; set; }
        public string? TimeSpent { get; set; }
        public string? Status { get; set; }
        public string? DateOfPassage { get; set; }
        public int? Score { get; set; }
        public int? CntQuestion { get; set; }
        public string? TestName { get; set; }
        public string? CategoryName { get; set; }
        public string? TestId { get; set; }
        public string? UserId { get; set; }
        public List<AnsweredQuestionWithoutPassedTestIdViewModel> AnsweredQuestions { get; set; }
    }
}

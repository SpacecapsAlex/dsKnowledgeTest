﻿namespace dsKnowledgeTest.ViewModels.AnsweredQuestionViewModels
{
    public class AnsweredQuestionViewModel
    {
        public string Id { get; set; }
        public List<string?>? ListSelectedAnswers { get; set; }
        public int? Score { get; set; }
        public string? PassedTestsId { get; set; }
        public string? QuestionId { get; set; }
    }
}
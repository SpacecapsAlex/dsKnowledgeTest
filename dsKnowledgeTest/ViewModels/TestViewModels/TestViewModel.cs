﻿using dsKnowledgeTest.Constants;

namespace dsKnowledgeTest.ViewModels.TestViewModel;

public class TestViewModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? TestLevel { get; set; }
    public bool? IsTestOnTime { get; set; }
    public int? TimeForTest { get; set; }
    public int? Score { get; set; }
    public int? CntQuestion { get; set; }
    public string? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public bool? IsRandomQuestions { get; set; }
    public bool? IsRandomAnswers { get; set; }
}
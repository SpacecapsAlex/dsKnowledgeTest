using dsKnowledgeTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace dsKnowledgeTest.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [Route("GetAllByTest")]
    [HttpGet]
    public async Task<ObjectResult> GetAllByTest(string testId)
    {
        var testGuid = Guid.Parse(testId);
        var result = await _questionService.GetAllQuestionForTestAsync(testGuid);
        return Ok(result);
    }
}
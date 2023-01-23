using dsKnowledgeTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace dsKnowledgeTest.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ITestService _testService;

    public TestController(ITestService testService)
    {
        _testService = testService;
    }

    [Route("GetAllByCategory")]
    [HttpGet]
    public async Task<ObjectResult> GetAllByCategory(string categoryId)
    {
        var categoryGuid = Guid.Parse(categoryId);
        var tests = await _testService.GetAllTestByCategoryAsync(categoryGuid);
        return Ok(tests);
    }
}
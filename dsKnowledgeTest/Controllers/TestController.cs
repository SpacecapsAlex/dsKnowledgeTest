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

    [HttpGet]
    public async Task<ObjectResult> Get()
    {
        var tests = await _testService.GetAllTestsAsync();
        return Ok(tests);

    }
}
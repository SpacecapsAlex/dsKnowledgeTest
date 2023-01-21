using Microsoft.AspNetCore.Mvc;

namespace dsKnowledgeTest.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    // GET
    public string Index()
    {
        return "TestIndex";
    }
}
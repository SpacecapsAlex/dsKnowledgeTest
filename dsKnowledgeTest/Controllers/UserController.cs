using Microsoft.AspNetCore.Mvc;

namespace dsKnowledgeTest.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    // GET
    public string Index()
    {
        return "UserIndex";
    }
}
using dsKnowledgeTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace dsKnowledgeTest.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [Route("GetAll")]
    [HttpGet]
    public async Task<ObjectResult> GetAll()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        return Ok(result);
    }
}
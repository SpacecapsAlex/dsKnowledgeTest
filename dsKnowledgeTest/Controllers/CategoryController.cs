using System.Net.Http.Headers;
using dsKnowledgeTest.Constants;
using dsKnowledgeTest.Services;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = nameof(RolesConst.User))]
    //[Authorize]
    [Route("GetAll")]
    [HttpGet]
    public async Task<ObjectResult> GetAll()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        return Ok(result);
    }
}
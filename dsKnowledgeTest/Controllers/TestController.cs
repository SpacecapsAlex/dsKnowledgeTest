using dsKnowledgeTest.Constants;
using dsKnowledgeTest.Services;
using dsKnowledgeTest.ViewModels.TestViewModel;
using dsKnowledgeTest.ViewModels.TestViewModels;
using Microsoft.AspNetCore.Authorization;
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

    [Route("GetTestById")]
    [HttpGet]
    public async Task<ObjectResult> GetTestById(string testId)
    {
        var test = await _testService.GetTestByIdAsync(Guid.Parse(testId));
        return Ok(test);
    }

    [Authorize(Roles = nameof(RolesConst.Admin))]
    [Route("Create")]
    [HttpPost]
    public async Task<ObjectResult> Create(CreateTestViewModel test)
    {
        try
        {
            await _testService.CreateTestAsync(test);
            return Ok("Данные добавлены");
        }
        catch
        {
            return BadRequest("Произошла ошибка");
        }
    }

    [Authorize(Roles = nameof(RolesConst.Admin))]
    [Route("CreateTestWithQuestion")]
    [HttpPost]
    public async Task<ObjectResult> CreateTestWithQuestion(CreateTestWithQuestionsViewModel test)
    {
        try
        {
            await _testService.CreateTestWithQuestionsAsync(test);
            return Ok("Данные добавлены");
        }
        catch
        {
            return BadRequest("Произошла ошибка");
        }
    }

     [Authorize(Roles = nameof(RolesConst.Admin))]
    [Route("EditTestWithQuestion")]
    [HttpPost]
    public async Task<ObjectResult> EditTestWithQuestion(EditTestWithQuestionsViewModel test)
    {
        try
        {
            await _testService.EditTestWithQuestionsAsync(test);
            return Ok("Данные добавлены");
        }
        catch
        {
            return BadRequest("Произошла ошибка");
        }
    }

    [Authorize(Roles = nameof(RolesConst.Admin))]
    [Route("Edit")]
    [HttpPost]
    public async Task<ObjectResult> Edit(EditTestViewModel test)
    {
        try
        {
            await _testService.EditTestAsync(test);
            return Ok("Данные обновлены");
        }
        catch
        {
            return BadRequest("Произошла ошибка");
        }
    }

    [Authorize(Roles = nameof(RolesConst.Admin))]
    [Route("Delete")]
    [HttpDelete]
    public async Task<ObjectResult> Delete(string testId)
    {
        try
        {
            await _testService.DeleteTestAsync(Guid.Parse(testId));
            return Ok("Данные удалены");
        }
        catch
        {
            return BadRequest("Произошла ошибка");
        }
    }
}
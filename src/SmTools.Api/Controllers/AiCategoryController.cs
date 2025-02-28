using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmTools.Api.Application.AiCategories;
using SmTools.Api.Model.AiCategories.Dtos;

namespace SmTools.Api.Controllers;

/// <summary>
/// AI 网站分类
/// </summary>
[ApiController]
[Route("ai/category")]
[Authorize]
public class AiCategoryController : ControllerBase
{
    private readonly IAiCategoryAppService _aiCategoryAppService;

    public AiCategoryController(IAiCategoryAppService aiCategoryAppService)
    {
        _aiCategoryAppService = aiCategoryAppService;
    }

    /// <summary>
    /// 新建/编辑 AI 网站分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddOrUpdate(AddOrUpdateAiCategoryInput input)
    {
        var result = await _aiCategoryAppService.AddOrUpdate(input);
        return Ok(result);
    }

    /// <summary>
    /// 分页查询 AI 网站分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("page")]
    public async Task<IActionResult> QueryPageAsync(QueryAiCategoryPageInput input)
    {
        var result = await _aiCategoryAppService.QueryPageAsync(input);
        return Ok(result);
    }
}
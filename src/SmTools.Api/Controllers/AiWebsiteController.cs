using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmTools.Api.Application.AiWebsites;
using SmTools.Api.Model.AiWebsites.Dtos;

namespace SmTools.Api.Controllers;

/// <summary>
/// AI 网站
/// </summary>
[ApiController]
[Route("ai/website")]
[Authorize]
public class AiWebsiteController : ControllerBase
{
    private readonly IAiWebsiteAppService _aiWebsiteAppService;

    public AiWebsiteController(IAiWebsiteAppService aiWebsiteAppService)
    {
        _aiWebsiteAppService = aiWebsiteAppService;
    }

    /// <summary>
    /// 新建/编辑 AI 网站
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddOrUpdate(AddOrUpdateAiWebsiteInput input)
    {
        var result = await _aiWebsiteAppService.AddOrUpdate(input);
        return Ok(result);
    }

    /// <summary>
    /// 分页查询 AI 网站
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("page")]
    public async Task<IActionResult> QueryPageAsync(QueryAiWebsitePageInput input)
    {
        var result = await _aiWebsiteAppService.QueryPageAsync(input);
        return Ok(result);
    }
}
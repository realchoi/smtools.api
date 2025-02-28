using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmTools.Api.Application.AiFavorites;
using SmTools.Api.Model.AiFavorites.Dtos;

namespace SmTools.Api.Controllers;

/// <summary>
/// AI 网站收藏
/// </summary>
[ApiController]
[Route("ai/favorite")]
[Authorize]
public class AiFavoriteController : ControllerBase
{
    private readonly IAiFavoriteAppService _aiFavoriteAppService;

    public AiFavoriteController(IAiFavoriteAppService aiFavoriteAppService)
    {
        _aiFavoriteAppService = aiFavoriteAppService;
    }

    /// <summary>
    /// 新建/编辑 AI 网站收藏
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddOrUpdate(AddOrUpdateAiFavoriteInput input)
    {
        var result = await _aiFavoriteAppService.AddOrUpdate(input);
        return Ok(result);
    }

    /// <summary>
    /// 分页查询 AI 网站收藏
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("page")]
    public async Task<IActionResult> QueryPageAsync(QueryAiFavoritePageInput input)
    {
        var result = await _aiFavoriteAppService.QueryPageAsync(input);
        return Ok(result);
    }
}
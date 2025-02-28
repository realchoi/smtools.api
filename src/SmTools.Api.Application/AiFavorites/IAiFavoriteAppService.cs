using SmTools.Api.Model;
using SmTools.Api.Model.AiFavorites.Dtos;
using SmTools.Api.Model.AiWebsites.Dtos;

namespace SmTools.Api.Application.AiFavorites;

/// <summary>
/// AI 网站收藏服务接口
/// </summary>
public interface IAiFavoriteAppService
{
    /// <summary>
    /// 新建/编辑 AI 网站收藏
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> AddOrUpdate(AddOrUpdateAiFavoriteInput input);

    /// <summary>
    /// 分页查询 AI 网站收藏
    /// </summary>
    Task<PagedDto<AiWebsiteDto>> QueryPageAsync(QueryAiFavoritePageInput input);
}
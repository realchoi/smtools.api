using SmTools.Api.Model;
using SmTools.Api.Model.AiWebsites.Dtos;

namespace SmTools.Api.Application.AiWebsites;

/// <summary>
/// AI 网站服务接口
/// </summary>
public interface IAiWebsiteAppService
{
    /// <summary>
    /// 新建/编辑 AI 网站
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> AddOrUpdate(AddOrUpdateAiWebsiteInput input);
    
    /// <summary>
    /// 分页查询 AI 网站
    /// </summary>
    Task<PagedDto<AiWebsiteDto>> QueryPageAsync(QueryAiWebsitePageInput input);
}
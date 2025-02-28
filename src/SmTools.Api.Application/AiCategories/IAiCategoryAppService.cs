using SmTools.Api.Model;
using SmTools.Api.Model.AiCategories.Dtos;

namespace SmTools.Api.Application.AiCategories;

/// <summary>
/// AI 网站分类服务接口
/// </summary>
public interface IAiCategoryAppService
{
    /// <summary>
    /// 新建/编辑 AI 网站分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> AddOrUpdate(AddOrUpdateAiCategoryInput input);

    /// <summary>
    /// 分页查询 AI 网站分类
    /// </summary>
    Task<PagedDto<AiCategoryDto>> QueryPageAsync(QueryAiCategoryPageInput input);
}
namespace SmTools.Api.Model.AiWebsites.Dtos;

/// <summary>
/// 查询 AI 网站分页输入
/// </summary>
public class QueryAiWebsitePageInput : PagedInput
{
    /// <summary>
    /// 分类 Id
    /// </summary>
    public string CategoryId { get; set; }

    /// <summary>
    /// 搜索关键词
    /// </summary>
    public string? Keyword { get; set; }
}
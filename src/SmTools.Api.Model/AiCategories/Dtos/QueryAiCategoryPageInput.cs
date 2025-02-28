namespace SmTools.Api.Model.AiCategories.Dtos;

/// <summary>
/// 查询 AI 网站分类分页输入
/// </summary>
public class QueryAiCategoryPageInput : PagedInput
{
    /// <summary>
    /// 搜索关键词
    /// </summary>
    public string? Keyword { get; set; }
}
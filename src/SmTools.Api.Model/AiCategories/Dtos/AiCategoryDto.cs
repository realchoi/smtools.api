namespace SmTools.Api.Model.AiCategories.Dtos;

/// <summary>
/// AI 网站分类 Dto
/// </summary>
public class AiCategoryDto : IdInput<string>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }
}
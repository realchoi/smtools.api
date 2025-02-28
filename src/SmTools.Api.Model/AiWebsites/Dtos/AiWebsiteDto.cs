namespace SmTools.Api.Model.AiWebsites.Dtos;

/// <summary>
/// AI 网站 Dto
/// </summary>
public class AiWebsiteDto : IdInput<string>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    public List<string> Category { get; set; }

    /// <summary>
    /// 网站地址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 网站 Logo
    /// </summary>
    public string Logo { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    public string Tags { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }
}
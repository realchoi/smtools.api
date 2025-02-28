namespace SmTools.Api.Model.AiFavorites.Dtos;

/// <summary>
/// 收藏网站 Dto
/// </summary>
public class AiFavoriteDto : IdInput<string>
{
    /// <summary>
    /// 用户 Id，关联 user_info 表的主键
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 网站 Id，关联 ai_website 表的主键
    /// </summary>
    public string WebsiteId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }
}
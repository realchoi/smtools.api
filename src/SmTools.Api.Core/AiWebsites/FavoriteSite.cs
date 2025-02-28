using Microsoft.EntityFrameworkCore;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.AiWebsites;

/// <summary>
/// 收藏网站表
/// </summary>
[Comment("收藏网站表")]
public class FavoriteSite : Entity<long>, IHasTimeAuditing
{
    /// <summary>
    /// 用户 Id，关联 user_info 表的主键
    /// </summary>
    [Comment("用户 Id，关联 user_info 表的主键")]
    public long UserId { get; set; }

    /// <summary>
    /// 网站 Id，关联 ai_website 表的主键
    /// </summary>
    [Comment("网站 Id，关联 ai_website 表的主键")]
    public long WebsiteId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Comment("创建时间")]
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Comment("修改时间")]
    public DateTime? ModificationTime { get; set; }
}
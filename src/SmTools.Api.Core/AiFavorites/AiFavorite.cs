using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.AiFavorites;

/// <summary>
/// 收藏网站表
/// </summary>
[Table("ai_favorite")]
[Comment("收藏网站表")]
public class AiFavorite : Entity<long>, IHasTimeAuditing
{
    /// <summary>
    /// 创建收藏网站
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <param name="websiteId"></param>
    public AiFavorite(long id, long userId, long websiteId)
    {
        this.Id = id;
        this.UserId = userId;
        this.WebsiteId = websiteId;
        this.CreationTime = DateTime.Now;
        this.ModificationTime = DateTime.Now;
    }

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
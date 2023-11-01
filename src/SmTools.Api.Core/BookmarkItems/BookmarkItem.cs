using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.BookmarkItems;

/// <summary>
/// 书签条目
/// </summary>
public class BookmarkItem : Entity<long>, IHasTimeAuditing
{
    /// <summary>
    /// 书签名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 书签网址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 所属分类目录的 id，关联 bookmark_category 表的主键
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// 用户 Id，关联 public.user_info 表的主键
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }
}
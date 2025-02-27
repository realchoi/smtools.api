using Microsoft.EntityFrameworkCore;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.BookmarkCategories;

/// <summary>
/// 书签分类目录
/// </summary>
[Comment("书签分类目录")]
public class BookmarkCategory : Entity<long>, IHasTimeAuditing
{
    public BookmarkCategory()
    {
    }

    /// <summary>
    /// 创建分类目录
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="userId"></param>
    /// <param name="parentId"></param>
    public BookmarkCategory(long id, string name, long userId, long parentId)
    {
        this.Id = id;
        this.Name = name;
        this.UserId = userId;
        this.ParentId = parentId;
        this.CreationTime = DateTime.Now;
        this.ModificationTime = DateTime.Now;
    }

    /// <summary>
    /// 文件夹名称
    /// </summary>
    [Comment("文件夹名称")]
    public string Name { get; set; }

    /// <summary>
    /// 父级 id
    /// </summary>
    [Comment("父级 id")]
    public long? ParentId { get; set; }

    /// <summary>
    /// 用户 Id，关联 public.user_info 表的主键
    /// </summary>
    [Comment("用户 Id，关联 public.user_info 表的主键")]
    public long UserId { get; set; }

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
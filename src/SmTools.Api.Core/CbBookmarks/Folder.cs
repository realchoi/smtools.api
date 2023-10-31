using System.ComponentModel.DataAnnotations.Schema;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.CbBookmarks;

/// <summary>
/// 书签文件夹
/// </summary>
[Table("folder", Schema = "cbbm")]
public class Folder : Entity<long>, IHasTimeAuditing
{
    /// <summary>
    /// 文件夹名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 父级 id
    /// </summary>
    public long? ParentId { get; set; }

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
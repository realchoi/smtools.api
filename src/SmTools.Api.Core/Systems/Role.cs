using Microsoft.EntityFrameworkCore;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.Systems;

/// <summary>
/// 角色表
/// </summary>
[Comment("角色表")]
public class Role : Entity<long>, IHasTimeAndUserAuditing
{
    /// <summary>
    /// 名称
    /// </summary>
    [Comment("名称")]
    public string Name { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Comment("备注")]
    public string Remark { get; set; }

    // [Navigate(typeof(RoleUser), nameof(RoleUser.RoleId), nameof(RoleUser.UserId))]//注意顺序
    // public List<User> UserList { get; set; }//只能是null不能赋默认值

    /// <summary>
    /// 创建用户
    /// </summary>
    [Comment("创建用户")]
    public string CreationUser { get; set; }

    /// <summary>
    /// 修改用户
    /// </summary>
    [Comment("修改用户")]
    public string? ModificationUser { get; set; }

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
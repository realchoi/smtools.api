using Microsoft.EntityFrameworkCore;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.Systems;

/// <summary>
/// [角色-权限]关系表
/// </summary>
[Comment("角色-权限关系表")]
public class RolePermission : Entity<long>, IHasTimeAndUserAuditing
{
    /// <summary>
    /// 权限表的 ID
    /// </summary>
    [Comment("权限表的 ID")]
    public string PermissionId { get; set; }

    /// <summary>
    /// 角色表的 ID
    /// </summary>
    [Comment("角色表的 ID")]
    public string RoleId { get; set; }

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
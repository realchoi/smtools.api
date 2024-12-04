using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.Systems;

/// <summary>
/// [权限组-权限]关系表
/// </summary>
public class PermissionGroupPermission : Entity<long>, IHasTimeAndUserAuditing
{
    /// <summary>
    /// 权限组表的 ID
    /// </summary>
    public string PermissionGroupId { get; set; }

    /// <summary>
    /// 权限表的 ID
    /// </summary>
    public string PermissionId { get; set; }

    /// <summary>
    /// 创建用户
    /// </summary>
    public string CreationUser { get; set; }

    /// <summary>
    /// 修改用户
    /// </summary>
    public string? ModificationUser { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }
}
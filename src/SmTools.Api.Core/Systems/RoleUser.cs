using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.Systems;

/// <summary>
/// [角色-用户]关系表
/// </summary>
public class RoleUser : Entity<long>, IHasTimeAndUserAuditing
{
    /// <summary>
    /// 用户表（Accounts 下面的 UserInfo）的 ID
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 角色表的 ID
    /// </summary>
    public string RoleId { get; set; }

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
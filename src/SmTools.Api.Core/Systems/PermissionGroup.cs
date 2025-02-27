using Microsoft.EntityFrameworkCore;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.Systems;

/// <summary>
/// 权限组表
/// </summary>
[Comment("权限组表")]
public class PermissionGroup : Entity<long>, IHasTimeAndUserAuditing
{
    /// <summary>
    /// 名称
    /// </summary>
    [Comment("名称")]
    public string Name { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    [Comment("代码")]
    public string Code { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Comment("备注")]
    public string Remark { get; set; }

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
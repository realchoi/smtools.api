using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.Systems;

/// <summary>
/// 角色表
/// </summary>
public class Role : Entity<long>, IHasTimeAndUserAuditing
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    // [Navigate(typeof(RoleUser), nameof(RoleUser.RoleId), nameof(RoleUser.UserId))]//注意顺序
    // public List<User> UserList { get; set; }//只能是null不能赋默认值

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
using Microsoft.EntityFrameworkCore;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.Accounts;

/// <summary>
/// 用户资料信息表
/// </summary>
[Comment("用户资料信息表")]
public class UserInfo : Entity<long>, IHasTimeAuditing
{
    /// <summary>
    /// 用户名（唯一）
    /// </summary>
    [Comment("用户名（唯一）")]
    public string UserName { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [Comment("昵称")]
    public string NickName { get; set; }

    /// <summary>
    /// 头像 URL
    /// </summary>
    [Comment("头像 URL")]
    public string? Avatar { get; set; }

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

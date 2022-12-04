using SmTools.Api.Model.Accounts;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.Accounts;

/// <summary>
/// 用户认证表
/// 参考：http://wpceo.com/user-database-table-design/
/// </summary>
public class UserAuth : Entity<long>, IHasTimeAuditing
{
    /// <summary>
    /// 用户 Id，关联 user_info 表的主键
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 登录类型
    /// </summary>
    public IdentityTypeEnum IdentityType { get; set; }

    /// <summary>
    /// 标识（手机号、邮箱、用户名或第三方应用的唯一标识）
    /// </summary>
    public string Identifier { get; set; }

    /// <summary>
    /// 密码凭证（站内的保存密码，站外的不保存或保存 token）
    /// </summary>
    public string Credential { get; set; }

    /// <summary>
    /// 密码加盐的盐值
    /// </summary>
    public string Salt { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }
}

namespace SmTools.Api.Model.Accounts.Dtos;

/// <summary>
/// 用户信息
/// </summary>
public class UserInfoDto
{
    /// <summary>
    /// 用户 Id，关联 user_info 表的主键
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 登录类型
    /// </summary>
    public IdentityTypeEnum IdentityType { get; set; }

    /// <summary>
    /// 标识（手机号、邮箱、用户名或第三方应用的唯一标识）
    /// </summary>
    public string Identifier { get; set; }

    /// <summary>
    /// 用户名（唯一）
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 头像 URL
    /// </summary>
    public string? Avatar { get; set; }
}
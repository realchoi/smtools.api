namespace SmTools.Api.Model.Accounts;

/// <summary>
/// 登录类型枚举
/// </summary>
public enum IdentityTypeEnum
{
    /// <summary>
    /// 手机号
    /// </summary>
    Phone = 1,

    /// <summary>
    /// 电子邮箱
    /// </summary>
    Email,

    /// <summary>
    /// 用户名
    /// </summary>
    UserName,

    /// <summary>
    /// 微信
    /// </summary>
    Wexin,

    /// <summary>
    /// 微博
    /// </summary>
    Weibo,

    /// <summary>
    /// Github
    /// </summary>
    Github
}

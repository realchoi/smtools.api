using System.ComponentModel;

namespace SmTools.Api.Model.Accounts;

/// <summary>
/// 登录类型枚举
/// </summary>
public enum IdentityTypeEnum
{
    /// <summary>
    /// 手机号
    /// </summary>
    [Description("手机号")]
    Phone = 1,

    /// <summary>
    /// 电子邮箱
    /// </summary>
    [Description("电子邮箱")]
    Email,

    /// <summary>
    /// 用户名
    /// </summary>
    [Description("用户名")]
    UserName,

    /// <summary>
    /// 微信
    /// </summary>
    [Description("微信")]
    Wexin,

    /// <summary>
    /// 微博
    /// </summary>
    [Description("微博")]
    Weibo,

    /// <summary>
    /// Github
    /// </summary>
    [Description("Github")]
    Github
}

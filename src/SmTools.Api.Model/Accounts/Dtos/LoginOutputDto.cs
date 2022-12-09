namespace SmTools.Api.Model.Accounts.Dtos;

/// <summary>
/// 用户登录出参
/// </summary>
public class LoginOutputDto
{
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

    /// <summary>
    /// 请求接口的 JWT
    /// </summary>
    public string AccessToken { get; set; }
}

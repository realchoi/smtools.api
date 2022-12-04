namespace SmTools.Api.Model.Accounts.Dtos;

/// <summary>
/// 用户登录出参
/// </summary>
public class LoginOutputDto
{
    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 头像 URL
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// JWT Token
    /// </summary>
    public string JwtToken { get; set; }
}

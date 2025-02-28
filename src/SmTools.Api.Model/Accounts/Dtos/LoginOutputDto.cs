namespace SmTools.Api.Model.Accounts.Dtos;

/// <summary>
/// 用户登录出参
/// </summary>
public class LoginOutputDto
{
    /// <summary>
    /// 请求接口的 JWT
    /// </summary>
    public string AccessToken { get; set; }
    
    /// <summary>
    /// 用户信息
    /// </summary>
    public UserInfoDto UserInfo { get; set; }
}
namespace SmTools.Api.Model.Account.Dtos;

/// <summary>
/// 用户登录入参
/// </summary>
public class LoginInputDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}

namespace SmTools.Api.Model.Account.dtos;

/// <summary>
/// 用户注册入参
/// </summary>
public class RegisterInputDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string EMail { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}

namespace SmTools.Api.Model.Accounts.Dtos;

/// <summary>
/// 用户注册出参
/// </summary>
public class RegisterOutputDto
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
    /// 密码凭证（站内的保存密码，站外的不保存或保存 token）
    /// </summary>
    public string Credential { get; set; }
}

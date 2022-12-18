namespace SmTools.Api.Model.Accounts.Dtos;

/// <summary>
/// 修改用户名入参
/// </summary>
public class ChangeUserNameInputDto
{
    /// <summary>
    /// 标识（当前用户名）
    /// </summary>
    public string Identifier { get; set; }

    /// <summary>
    /// 新用户名
    /// </summary>
    public string NewUserName { get; set; }
}

using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;

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

    /// <summary>
    /// 必填项验证
    /// </summary>
    /// <exception cref="InvalidParameterException"></exception>
    public void Validate()
    {
        if (Identifier.IsNullOrEmpty())
        {
            throw new InvalidParameterException("用户名不能为空");
        }

        if (NewUserName.IsNullOrEmpty())
        {
            throw new InvalidParameterException("新用户名不能为空");
        }
    }
}
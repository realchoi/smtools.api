using SmTools.Api.Model.Extensions;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;

namespace SmTools.Api.Model.Accounts.Dtos;

/// <summary>
/// 修改密码入参
/// </summary>
public class ChangePasswordInputDto
{
    /// <summary>
    /// 登录类型
    /// </summary>
    public IdentityTypeEnum IdentityType { get; set; }

    /// <summary>
    /// 标识（手机号、邮箱、用户名或第三方应用的唯一标识）
    /// </summary>
    public string Identifier { get; set; }

    /// <summary>
    /// 旧密码凭证（站内的保存密码，站外的不保存或保存 token）
    /// </summary>
    public string OldCredential { get; set; }

    /// <summary>
    /// 新密码凭证（站内的保存密码，站外的不保存或保存 token）
    /// </summary>
    public string NewCredential { get; set; }

    /// <summary>
    /// 必填项验证
    /// </summary>
    /// <exception cref="InvalidParameterException"></exception>
    public void Validate()
    {
        var identityTypeName = IdentityType.GetDescription();
        if (Identifier.IsNullOrEmpty())
        {
            throw new InvalidParameterException($"{identityTypeName}不能为空");
        }

        if (OldCredential.IsNullOrEmpty())
        {
            throw new InvalidParameterException("旧密码不能为空");
        }

        if (NewCredential.IsNullOrEmpty())
        {
            throw new InvalidParameterException("新密码不能为空");
        }
    }
}
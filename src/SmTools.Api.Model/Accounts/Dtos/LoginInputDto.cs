﻿using SmTools.Api.Model.Extensions;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;

namespace SmTools.Api.Model.Accounts.Dtos;

/// <summary>
/// 用户登录入参
/// </summary>
public class LoginInputDto
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
    /// 密码凭证（站内的保存密码，站外的不保存或保存 token）
    /// </summary>
    public string Credential { get; set; }

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

        if (Credential.IsNullOrEmpty())
        {
            throw new InvalidParameterException("密码不能为空");
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmTools.Api.Filters;
using SmTools.Api.Model;
using SpringMountain.Framework.Exceptions;

namespace SmTools.Api.Controllers;

/// <summary>
/// 公共接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class CommonController : ControllerBase
{
    /// <summary>
    /// 获取指定枚举的枚举值列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("enums")]
    public ListResultDto<NameValueDto<int>> GetEnums(string fullName)
    {
        var type = AppDomain.CurrentDomain.GetAssemblies()
            .Where(c => c.FullName?.StartsWith("SmTools.Api") ?? false)
            .SelectMany(c => c.GetTypes()).FirstOrDefault(c => c.FullName == fullName);
        if (type == null)
        {
            throw new NotFoundException("指定的枚举不存在");
        }
        return new ListResultDto<NameValueDto<int>>(SwaggerAddEnumDescriptionFilter.GetEnumNameValues(type));
    }

    /// <summary>
    /// 获取指定枚举的枚举值列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("const")]
    public ListResultDto<NameValueDto<string>> GetConst(string fullName)
    {
        var type = AppDomain.CurrentDomain.GetAssemblies()
            .Where(c => c.FullName?.StartsWith("SmTools.Api") ?? false)
            .SelectMany(c => c.GetTypes()).FirstOrDefault(c => c.FullName == fullName);
        if (type == null)
        {
            throw new NotFoundException("指定的枚举不存在");
        }
        return new ListResultDto<NameValueDto<string>>(SwaggerAddEnumDescriptionFilter.GetConstNameValues(type));
    }

    /// <summary>
    /// 获取指定枚举的枚举值列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("enums/all")]
    public ListResultDto<NameValueDto<string>> GetAllEnums()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(c => c.FullName?.StartsWith("SmTools.Api") ?? false)
            .SelectMany(c => c.GetTypes().Where(x => x.IsEnum));

        var res = types.Select(c => new NameValueDto<string>
        {
            Name = SwaggerAddEnumDescriptionFilter.AllEnumSummary
                 .FirstOrDefault(x => x.name == $"T:{(c.FullName ?? string.Empty).Replace("+", ".")}")
                 .summary ?? c.Name,
            Value = c.FullName
        }).ToList();
        return new ListResultDto<NameValueDto<string>>(res);
    }
}

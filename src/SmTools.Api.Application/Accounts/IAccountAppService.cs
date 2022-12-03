using SmTools.Api.Model.Accounts.Dtos;

namespace SmTools.Api.Application.Accounts;

/// <summary>
/// 用户账号服务接口
/// </summary>
public interface IAccountAppService
{
    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    Task<RegisterOutputDto> Register(RegisterInputDto registerDto);

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    Task<LoginOutputDto> Login(LoginInputDto login);
}

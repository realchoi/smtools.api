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
    /// <param name="registerInput"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    Task<RegisterOutputDto> Register(RegisterInputDto registerInput);

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="loginInput"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="InternalServerErrorException"></exception>
    Task<LoginOutputDto> Login(LoginInputDto loginInput);

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="changePasswordInput"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="InvalidParameterException"></exception>
    Task<ChangePasswordOutputDto> ChangePassword(long userId, ChangePasswordInputDto changePasswordInput);
}

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
    Task<RegisterOutputDto> Register(RegisterInputDto registerInput);

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="loginInput"></param>
    /// <returns></returns>
    Task<LoginOutputDto> Login(LoginInputDto loginInput);

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="changePasswordInput"></param>
    /// <returns></returns>
    Task<ChangePasswordOutputDto> ChangePassword(long userId, ChangePasswordInputDto changePasswordInput);

    /// <summary>
    /// 修改用户名
    /// </summary>
    /// <param name="userId">当前登录用户的 Id</param>
    /// <param name="changeUserNameInput"></param>
    /// <returns></returns>
    Task<ChangeUserNameOutputDto> ChangeUserName(long userId, ChangeUserNameInputDto changeUserNameInput);
}

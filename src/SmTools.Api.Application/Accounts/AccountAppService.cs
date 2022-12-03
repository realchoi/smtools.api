using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.Accounts;
using SmTools.Api.Core.Helpers;
using SmTools.Api.Model.Accounts.Dtos;
using SpringMountain.Framework.Domain.Repositories;
using SpringMountain.Framework.Exceptions;

namespace SmTools.Api.Application.Accounts;

/// <summary>
/// 用户账号服务实现类
/// </summary>
public class AccountAppService : IAccountAppService
{
    private readonly JwtHelper _jwtHelper;
    private readonly IRepository<UserAuth, long> _userAuthRepository;
    private readonly IRepository<UserInfo, long> _userInfoRepository;

    public AccountAppService(JwtHelper jwtHelper,
        IRepository<UserAuth, long> userAuthRepository,
        IRepository<UserInfo, long> userInfoRepository)
    {
        _jwtHelper = jwtHelper;
        _userAuthRepository = userAuthRepository;
        _userInfoRepository = userInfoRepository;
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="InternalServerErrorException"></exception>
    public async Task<LoginOutputDto> Login(LoginInputDto login)
    {
        var userAuth = await _userAuthRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.IdentityType == login.IdentityType
            && p.Identifier == login.Identifier && p.Credential == login.Credential);
        if (userAuth == null)
        {
            throw new NotFoundException("用户信息不正确");
        }
        var userInfo = await _userInfoRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.Id == userAuth.UserId);
        if (userInfo == null)
        {
            // 如果用户能认证成功，但却找不到用户信息，说明程序本身出了问题，这里抛出服务器内部异常
            throw new InternalServerErrorException("未找到用户信息");
        }
        // 认证成功后，生成一个 jwt
        var jwtToken = _jwtHelper.CreateToken();
        return new LoginOutputDto
        {
            NickName = userInfo.NickName,
            Avatar = userInfo.Avatar,
            JwtToken = jwtToken
        };
    }
}

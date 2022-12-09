using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.Accounts;
using SmTools.Api.Core.Helpers;
using SmTools.Api.Model.Accounts;
using SmTools.Api.Model.Accounts.Dtos;
using SpringMountain.Framework.Core.Exceptions;
using SpringMountain.Framework.Domain.Repositories;
using SpringMountain.Framework.Exceptions;
using SpringMountain.Framework.Snowflake;
using SpringMountain.Framework.Uow;

namespace SmTools.Api.Application.Accounts;

/// <summary>
/// 用户账号服务实现类
/// </summary>
public class AccountAppService : IAccountAppService
{
    private readonly JwtHelper _jwtHelper;
    private readonly ISnowflakeIdMaker _snowflakeIdMaker;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IRepository<UserAuth, long> _userAuthRepository;
    private readonly IRepository<UserInfo, long> _userInfoRepository;

    public AccountAppService(JwtHelper jwtHelper,
        ISnowflakeIdMaker snowflakeIdMaker,
        IUnitOfWorkManager unitOfWorkManager,
        IRepository<UserAuth, long> userAuthRepository,
        IRepository<UserInfo, long> userInfoRepository)
    {
        _jwtHelper = jwtHelper;
        _snowflakeIdMaker = snowflakeIdMaker;
        _unitOfWorkManager = unitOfWorkManager;
        _userAuthRepository = userAuthRepository;
        _userInfoRepository = userInfoRepository;
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="registerInput"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<RegisterOutputDto> Register(RegisterInputDto registerInput)
    {
        var exist = await _userAuthRepository.GetQueryable()
            .AnyAsync(p => p.IdentityType == registerInput.IdentityType && p.Identifier == registerInput.Identifier);
        if (exist)
        {
            throw new InvalidParameterException("当前用户已存在，请换一个后再试");
        }

        // 如果注册类型是用户名，则直接使用用户填入的用户名，否则随机生成一个
        var userName = registerInput.IdentityType == IdentityTypeEnum.UserName
            ? registerInput.Identifier
            : $"user_{UserNameHelper.GenRandomUserName(10)}";

        var userInfo = new UserInfo
        {
            Id = _snowflakeIdMaker.NextId(),
            UserName = userName,
            NickName = registerInput.NickName
        };
        await _userInfoRepository.AddAsync(userInfo);
        var rnd = new Random();
        var n = rnd.Next(100, 128);
        var salt = Base64Helper.Base64Encode(HashingHelper.GenerateSalt(n));
        var passwordHash = HashingHelper.HashUsingPbkdf2(registerInput.Credential, salt);
        var userAuth = new UserAuth
        {
            UserId = userInfo.Id,
            IdentityType = registerInput.IdentityType,
            Identifier = registerInput.Identifier,
            Credential = passwordHash,
            Salt = salt
        };
        await _userAuthRepository.AddAsync(userAuth);
        await _unitOfWorkManager.Current!.SaveChangesAsync();
        return new RegisterOutputDto
        {
            UserName = userName,
            NickName = registerInput.NickName,
            Credential = registerInput.Credential
        };
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="loginInput"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="InternalServerErrorException"></exception>
    public async Task<LoginOutputDto> Login(LoginInputDto loginInput)
    {
        var userAuth = await _userAuthRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.IdentityType == loginInput.IdentityType
            && p.Identifier == loginInput.Identifier);
        if (userAuth == null)
        {
            throw new NotFoundException("用户不存在");
        }
        var passwordHash = HashingHelper.HashUsingPbkdf2(loginInput.Credential, userAuth.Salt);
        if (userAuth.Credential != passwordHash)
        {
            throw new InvalidParameterException("密码不正确");
        }
        var userInfo = await _userInfoRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.Id == userAuth.UserId);
        if (userInfo == null)
        {
            // 如果用户能认证成功，但却找不到用户信息，说明程序本身出了问题，这里抛出服务器内部异常
            throw new InternalServerErrorException("未找到用户信息");
        }
        // 认证成功后，生成一个 jwt
        var jwtToken = _jwtHelper.CreateToken(userInfo);
        return new LoginOutputDto
        {
            UserName = userInfo.UserName,
            NickName = userInfo.NickName,
            Avatar = userInfo.Avatar,
            AccessToken = jwtToken
        };
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="changePasswordInput"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<ChangePasswordOutputDto> ChangePassword(ChangePasswordInputDto changePasswordInput)
    {
        var userAuth = await _userAuthRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.IdentityType == changePasswordInput.IdentityType
            && p.Identifier == changePasswordInput.Identifier);
        if (userAuth == null)
        {
            throw new NotFoundException("用户不存在");
        }
        var oldPasswordHash = HashingHelper.HashUsingPbkdf2(changePasswordInput.OldCredential, userAuth.Salt);
        if (userAuth.Credential != oldPasswordHash)
        {
            throw new InvalidParameterException("原始密码不正确");
        }
        // 重新生成盐值
        var rnd = new Random();
        var n = rnd.Next(1, 128);
        var newSalt = Base64Helper.Base64Encode(HashingHelper.GenerateSalt(n));
        var newPasswordHash = HashingHelper.HashUsingPbkdf2(changePasswordInput.NewCredential, newSalt);
        userAuth.Credential = newPasswordHash;
        userAuth.Salt = newSalt;
        await _unitOfWorkManager.Current!.SaveChangesAsync();
        return new ChangePasswordOutputDto
        {
            Identifier = userAuth.Identifier,
            NewCredential = changePasswordInput.NewCredential
        };
    }
}

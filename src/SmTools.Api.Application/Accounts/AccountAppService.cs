using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.Accounts;
using SmTools.Api.Core.Helpers;
using SmTools.Api.Model.Accounts;
using SmTools.Api.Model.Accounts.Dtos;
using SmTools.Api.Model.Extensions;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Auth;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.NotFound;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;
using SpringMountain.Framework.Domain.Repositories;
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
    /// <exception cref="AlreadyExistedException"></exception>
    public async Task<RegisterOutputDto> Register(RegisterInputDto registerInput)
    {
        var exist = await _userAuthRepository.GetQueryable()
            .AnyAsync(p => p.IdentityType == registerInput.IdentityType && p.Identifier == registerInput.Identifier);
        if (exist)
        {
            throw new AlreadyExistedException($"当前{registerInput.IdentityType.GetDescription()}已存在，请换一个后再试");
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
        var userAuths = new List<UserAuth>
        {
            // 用户提交的注册类型
            new UserAuth
            {
                UserId = userInfo.Id,
                IdentityType = registerInput.IdentityType,
                Identifier = registerInput.Identifier,
                Credential = passwordHash,
                Salt = salt
            }
        };
        // 如果用户提交的注册类型不是用户名注册，则同步新增一个用户名登录，使后续可以使用用户名登录
        if (registerInput.IdentityType != IdentityTypeEnum.UserName)
        {
            userAuths.Add(new UserAuth
            {
                UserId = userInfo.Id,
                IdentityType = IdentityTypeEnum.UserName,
                Identifier = userName,
                Credential = passwordHash,
                Salt = salt
            });
        }
        await _userAuthRepository.AddAsync(userAuths);
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
    /// <exception cref="WrongPasswordException"></exception>
    public async Task<LoginOutputDto> Login(LoginInputDto loginInput)
    {
        var userAuth = await _userAuthRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.IdentityType == loginInput.IdentityType
            && p.Identifier == loginInput.Identifier);
        if (userAuth == null)
        {
            throw new NotFoundException($"{loginInput.IdentityType.GetDescription()}不存在");
        }
        var passwordHash = HashingHelper.HashUsingPbkdf2(loginInput.Credential, userAuth.Salt);
        if (userAuth.Credential != passwordHash)
        {
            throw new WrongPasswordException("密码不正确");
        }
        var userInfo = await _userInfoRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.Id == userAuth.UserId);
        if (userInfo == null)
        {
            throw new NotFoundException("未找到用户信息");
        }
        // 认证成功后，生成一个 jwt
        var accessToken = _jwtHelper.CreateToken(userInfo);
        return new LoginOutputDto
        {
            IdentityType = loginInput.IdentityType,
            Identifier = loginInput.Identifier,
            UserName = userInfo.UserName,
            NickName = userInfo.NickName,
            Avatar = userInfo.Avatar,
            AccessToken = accessToken
        };
    }


    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="userId">当前登录用户的 Id</param>
    /// <param name="changePasswordInput"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ForbiddenException"></exception>
    /// <exception cref="UnauthenticatedException"></exception>
    public async Task<ChangePasswordOutputDto> ChangePassword(long userId, ChangePasswordInputDto changePasswordInput)
    {
        var userAuth = await _userAuthRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.IdentityType == changePasswordInput.IdentityType
            && p.Identifier == changePasswordInput.Identifier);
        if (userAuth == null)
        {
            throw new NotFoundException($"{changePasswordInput.IdentityType.GetDescription()}不存在");
        }
        if (userAuth.UserId != userId)
        {
            throw new ForbiddenException("没有权限修改他人的账户密码");
        }
        var oldPasswordHash = HashingHelper.HashUsingPbkdf2(changePasswordInput.OldCredential, userAuth.Salt);
        if (userAuth.Credential != oldPasswordHash)
        {
            throw new UnauthenticatedException("旧密码不正确");
        }
        // 重新生成盐值
        var rnd = new Random();
        var n = rnd.Next(100, 128);
        var newSalt = Base64Helper.Base64Encode(HashingHelper.GenerateSalt(n));
        var newPasswordHash = HashingHelper.HashUsingPbkdf2(changePasswordInput.NewCredential, newSalt);
        userAuth.Credential = newPasswordHash;
        userAuth.Salt = newSalt;

        // 还需要同步修改同一个用户的其他登录方式的登录密码
        var otherUserAuths = await _userAuthRepository.GetQueryable()
            .Where(p => p.UserId == userAuth.UserId && p.IdentityType != changePasswordInput.IdentityType).ToListAsync();
        otherUserAuths.ForEach(u =>
        {
            u.Credential = newPasswordHash;
            u.Salt = newSalt;
        });
        await _unitOfWorkManager.Current!.SaveChangesAsync();
        return new ChangePasswordOutputDto
        {
            Identifier = userAuth.Identifier,
            NewCredential = changePasswordInput.NewCredential
        };
    }

    /// <summary>
    /// 修改用户名
    /// </summary>
    /// <param name="userId">当前登录用户的 Id</param>
    /// <param name="changeUserNameInput"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="ForbiddenException"></exception>
    public async Task<ChangeUserNameOutputDto> ChangeUserName(long userId, ChangeUserNameInputDto changeUserNameInput)
    {
        var userInfo = await _userInfoRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.UserName == changeUserNameInput.Identifier);
        if (userInfo == null)
        {
            throw new NotFoundException($"用户名 {changeUserNameInput.Identifier} 不存在");
        }
        if (userInfo.Id != userId)
        {
            throw new ForbiddenException("没有权限修改他人的用户名");
        }
        userInfo.UserName = changeUserNameInput.NewUserName;

        var userAuth = await _userAuthRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.IdentityType == IdentityTypeEnum.UserName
            && p.Identifier == changeUserNameInput.Identifier);
        if (userAuth != null)
        {
            if (userAuth.UserId != userId)
            {
                throw new ForbiddenException("没有权限修改他人的用户名");
            }
            userAuth.Identifier = changeUserNameInput.Identifier;
        }
        await _unitOfWorkManager.Current!.SaveChangesAsync();
        return new ChangeUserNameOutputDto
        {
            NewUserName = changeUserNameInput.NewUserName
        };
    }
}

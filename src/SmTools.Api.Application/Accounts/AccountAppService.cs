﻿using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.Accounts;
using SmTools.Api.Core.Helpers;
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
    /// <param name="registerDto"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<RegisterOutputDto> Register(RegisterInputDto registerDto)
    {
        var exist = await _userAuthRepository.GetQueryable()
            .AnyAsync(p => p.IdentityType == registerDto.IdentityType
            && p.Identifier == registerDto.Identifier && p.Credential == registerDto.Credential);
        if (exist)
        {
            throw new InvalidParameterException("当前用户已存在，请换一个后再试");
        }
        var userInfo = new UserInfo
        {
            Id = _snowflakeIdMaker.NextId(),
            UserName = registerDto.UserName,
            NickName = registerDto.NickName
        };
        await _userInfoRepository.AddAsync(userInfo);
        var passwordHash = HashingHelper.HashUsingPbkdf2(registerDto.Credential, "smtools");
        var userAuth = new UserAuth
        {
            UserId = userInfo.Id,
            IdentityType = registerDto.IdentityType,
            Identifier = registerDto.Identifier,
            Credential = passwordHash
        };
        await _userAuthRepository.AddAsync(userAuth);
        await _unitOfWorkManager.Current!.SaveChangesAsync();
        return new RegisterOutputDto
        {
            UserName = registerDto.UserName,
            NickName = registerDto.NickName,
            Credential = registerDto.Credential
        };
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="InternalServerErrorException"></exception>
    public async Task<LoginOutputDto> Login(LoginInputDto loginDto)
    {
        var userAuth = await _userAuthRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.IdentityType == loginDto.IdentityType
            && p.Identifier == loginDto.Identifier);
        if (userAuth == null)
        {
            throw new NotFoundException("用户不存在");
        }
        var passwordHash = HashingHelper.HashUsingPbkdf2(loginDto.Credential, "smtools");
        if (loginDto.Credential != passwordHash)
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
            NickName = userInfo.NickName,
            Avatar = userInfo.Avatar,
            JwtToken = jwtToken
        };
    }
}
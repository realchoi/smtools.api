using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmTools.Api.Application.Accounts;
using SmTools.Api.Core.Accounts;
using SmTools.Api.Core.Helpers;
using SmTools.Api.Model.Accounts.Dtos;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Auth;
using SpringMountain.Framework.Snowflake;
using System.Security.Claims;
using SmTools.Api.Filters;

namespace SmTools.Api.Controllers;

/// <summary>
/// 账户
/// </summary>
[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{
    private readonly JwtHelper _jwtHelper;
    private readonly ISnowflakeIdMaker _snowflakeIdMaker;
    private readonly IAccountAppService _accountAppService;

    public AccountController(JwtHelper jwtHelper,
        ISnowflakeIdMaker snowflakeIdMaker,
        IAccountAppService accountAppService)
    {
        _jwtHelper = jwtHelper;
        _snowflakeIdMaker = snowflakeIdMaker;
        _accountAppService = accountAppService;
    }

    /// <summary>
    /// 获取 jwt token
    /// </summary>
    /// <returns></returns>
    [HttpGet("token")]
    public ActionResult<string> GetToken()
    {
        return _jwtHelper.CreateToken(new UserInfo()
        {
            Id = 1000000000000000001,
            UserName = "test_user",
            NickName = "测试用户"
        });
    }

    /// <summary>
    /// 测试认证
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("authenticate/test")]
    public ActionResult<string> TestAuthenticate()
    {
        return "看到此信息，说明已经认证成功";
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="registerInput"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [PreventRepeatSubmit]
    public async Task<JsonResult> Register(RegisterInputDto registerInput)
    {
        var result = await _accountAppService.Register(registerInput);
        return new JsonResult(result);
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="loginInput"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<JsonResult> Login(LoginInputDto loginInput)
    {
        var result = await _accountAppService.Login(loginInput);
        return new JsonResult(result);
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="changePasswordInput"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("password/change")]
    [PreventRepeatSubmit]
    public async Task<IActionResult> ChangePassword(ChangePasswordInputDto changePasswordInput)
    {
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
        {
            throw new WrongUserPassException("账号错误");
        }

        if (!long.TryParse(claim.Value, out var userId))
        {
            throw new WrongUserPassException("账号错误");
        }

        var result = await _accountAppService.ChangePassword(userId, changePasswordInput);
        return new JsonResult(result);
    }

    /// <summary>
    /// 修改用户名
    /// </summary>
    /// <param name="changeUserNameInput"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("username/change")]
    [PreventRepeatSubmit]
    public async Task<IActionResult> ChangeUserName(ChangeUserNameInputDto changeUserNameInput)
    {
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
        {
            throw new WrongUserPassException("账号错误");
        }

        if (!long.TryParse(claim.Value, out var userId))
        {
            throw new WrongUserPassException("账号错误");
        }

        var result = await _accountAppService.ChangeUserName(userId, changeUserNameInput);
        return new JsonResult(result);
    }
}
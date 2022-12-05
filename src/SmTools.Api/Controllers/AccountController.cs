using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmTools.Api.Application.Accounts;
using SmTools.Api.Core.Helpers;
using SmTools.Api.Model.Accounts.Dtos;
using SpringMountain.Framework.Core.Exceptions;
using SpringMountain.Framework.Exceptions;
using SpringMountain.Framework.Snowflake;

namespace SmTools.Api.Controllers;

/// <summary>
/// 账户
/// </summary>
[ApiController]
[Route("[controller]")]
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
        return _jwtHelper.CreateToken(new Core.Accounts.UserInfo() { NickName = "test_user" });
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
    [HttpPost("password/change")]
    public async Task<JsonResult> ChangePassword(ChangePasswordInputDto changePasswordInput)
    {
        var result = await _accountAppService.ChangePassword(changePasswordInput);
        return new JsonResult(result);
    }
}

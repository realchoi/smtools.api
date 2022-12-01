using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmTools.Api.Helpers;
using SmTools.Api.Model.Account.Dtos;
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

    public AccountController(JwtHelper jwtHelper, ISnowflakeIdMaker snowflakeIdMaker)
    {
        _jwtHelper = jwtHelper;
        _snowflakeIdMaker = snowflakeIdMaker;
    }

    /// <summary>
    /// 获取 jwt token
    /// </summary>
    /// <returns></returns>
    [HttpGet("token")]
    public ActionResult<string> GetToken()
    {
        return _jwtHelper.CreateToken();
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
    /// <param name="inputDto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<JsonResult> Register(RegisterInputDto inputDto)
    {
        var id = _snowflakeIdMaker.NextId();
        Console.WriteLine(id);
        return new JsonResult(new { Id = id });
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="inputDto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<JsonResult> Login(LoginInputDto inputDto)
    {
        return new JsonResult(inputDto);
    }
}

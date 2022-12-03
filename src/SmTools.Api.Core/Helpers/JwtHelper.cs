using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmTools.Api.Core.Helpers;

/// <summary>
/// jwt 签发
/// </summary>
public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 生成 jwt token
    /// </summary>
    /// <returns></returns>
    public string CreateToken()
    {
        // 1. 定义需要使用到的 Claims
        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Name, "admin"),    // HttpContext.User.Identity.Name
            new Claim(ClaimTypes.Role, "admin"),    // HttpContext.User.IsInRole("admin")
            new Claim(JwtRegisteredClaimNames.Jti, "admin"),
            new Claim("UserName", "Admin"),
            new Claim("Name", "超级管理员")
        };

        // 2. 从 appsettings.json 中读取 SecretKey
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        // 3. 选择加密算法
        var algorithm = SecurityAlgorithms.HmacSha256;

        // 4. 生成 Credentials
        var signingCredentials = new SigningCredentials(secretKey, algorithm);

        // 5. 根据以上，生成 token
        var jwtToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                DateTime.Now,
                DateTime.Now.AddSeconds(300),
                signingCredentials
            );

        // 6. 将 token 转为 string
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }
}

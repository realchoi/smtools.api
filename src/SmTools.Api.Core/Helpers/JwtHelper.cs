using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmTools.Api.Core.Accounts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
    /// <param name="userInfo">用户资料信息</param>
    /// <returns></returns>
    public string CreateToken(UserInfo userInfo)
    {
        // 1. 定义需要使用到的 Claims
        var claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
            new Claim(ClaimTypes.Name, userInfo.NickName),    // HttpContext.User.Identity.Name
            //new Claim(ClaimTypes.Role, "admin"),    // HttpContext.User.IsInRole("admin")
            //new Claim(JwtRegisteredClaimNames.Jti, "admin"),
            //new Claim("UserName", "Admin"),
            new Claim("Name", userInfo.NickName)
        };

        // 2. 从 appsettings.json 中读取 SecretKey
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        // 3. 选择加密算法
        var algorithm = SecurityAlgorithms.HmacSha256;

        // 4. 生成 Credentials
        var signingCredentials = new SigningCredentials(secretKey, algorithm);

        // 5. 根据以上，生成 token
        var jwtToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(300),
                signingCredentials: signingCredentials
            );

        // 6. 将 token 转为 string
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }

    /// <summary>
    /// 生成 jwt 的 SecretKey
    /// </summary>
    /// <returns></returns>
    public string GenerateSecureSecret()
    {
        var hmac = new HMACSHA256();
        return Convert.ToBase64String(hmac.Key);
    }
}

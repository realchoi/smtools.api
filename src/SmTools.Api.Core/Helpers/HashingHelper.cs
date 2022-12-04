using System.Security.Cryptography;
using System.Text;

namespace SmTools.Api.Core.Helpers;

/// <summary>
/// Hash 帮助类
/// </summary>
public class HashingHelper
{
    /// <summary>
    /// 密码加盐（使用 PBKDF2）
    /// </summary>
    /// <remarks>
    /// 我们使用的是基于派生函数（PBKDF2），它应用了 HMac 函数结合一个散列算法（sha - 256）将密码和盐值（base64 编码的随机数与大小 128 位）重复多次后作为迭代参数中指定的参数（是默认的 10000 倍）
    /// </remarks>
    /// <param name="password">原密码</param>
    /// <param name="salt">盐值</param>
    /// <returns>加盐后的密码</returns>
    public static string HashUsingPbkdf2(string password, string salt)
    {
        using var bytes = new Rfc2898DeriveBytes
        (password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
        var derivedRandomKey = bytes.GetBytes(32);
        var hash = Convert.ToBase64String(derivedRandomKey);
        return hash;
    }

    /// <summary>
    /// 获取盐值
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static string GetSalt(int n)
    {
        char[] chars = ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" +
                "1234567890!@#$%^&*()_+").ToCharArray();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < n; i++)
        {
            //Random().nextInt()返回值为[0,n)
            char aChar = chars[new Random().NextInt64(chars.Length)];
            sb.Append(aChar);
        }
        return sb.ToString();
    }
}

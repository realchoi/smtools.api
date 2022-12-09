using System.Text;

namespace SmTools.Api.Core.Helpers;

public class UserNameHelper
{
    /// <summary>
    /// 生成随机用户名
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GenRandomUserName(int length)
    {
        Random rd = new Random();
        byte[] str = new byte[length];
        int i;
        for (i = 0; i < length - 1; i++)
        {
            int a = 0;
            while (!((a >= 48 && a <= 57) || (a >= 97 && a <= 122)))
            {
                a = rd.Next(48, 122);
            }
            str[i] = (byte)a;
        }
        string username = new string(Encoding.ASCII.GetChars(str));
        Random r = new Random(unchecked((int)DateTime.Now.Ticks));
        string s1 = ((char)r.Next(97, 122)).ToString();
        // 防止存入 pg 数据库报错，详情见以下链接：
        // 1. https://stackoverflow.com/questions/1347646/postgres-error-on-insert-error-invalid-byte-sequence-for-encoding-utf8-0x0
        // 2. https://www.cnblogs.com/wggj/p/8194313.html
        username = username.Replace("/0", "").Replace(@"\0", "").Replace("\u0000", "");
        string randStr = s1 + username;
        return randStr;
    }
}

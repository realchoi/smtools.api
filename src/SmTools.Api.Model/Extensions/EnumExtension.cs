using System.ComponentModel;
using System.Reflection;

namespace SmTools.Api.Model.Extensions;

public static class EnumExtension
{
    /// <summary>
    /// 获取枚举描述
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string? GetDescription(this Enum value)
    {
        var enumType = value.GetType();
        var field = enumType.GetField(value.ToString());
        if (field == null) return null;
        if (field.IsDefined(typeof(DescriptionAttribute), false))
        {
            var desc = field.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
            return desc?.Description;
        }
        return null;
    }
}

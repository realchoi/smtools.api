namespace SmTools.Api.Model;

/// <summary>
/// 简单键值对
/// </summary>
/// <typeparam name="T"></typeparam>
public class NameValueDto<T>
{
    /// <summary>
    /// 描述
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public T Value { get; set; }
}

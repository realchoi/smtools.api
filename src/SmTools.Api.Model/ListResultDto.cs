namespace SmTools.Api.Model;

/// <summary>
/// 集合包装
/// </summary>
/// <typeparam name="T"></typeparam>
public class ListResultDto<T>
{
    /// <summary>
    /// 集合
    /// </summary>
    /// <param name="items"></param>
    public ListResultDto(IReadOnlyList<T> items)
    {
        Items = items;
    }

    /// <summary>
    /// 分页结果
    /// </summary>
    public IReadOnlyList<T> Items { get; private set; }
}

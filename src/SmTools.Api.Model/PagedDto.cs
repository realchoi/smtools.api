namespace SmTools.Api.Model;

/// <summary>
/// 分页入参
/// </summary>
public class PagedInput
{
    /// <summary>
    /// 第几页
    /// </summary>
    /// <example>1</example>
    public int PageIndex { get; set; } = 1;

    /// <summary>
    /// 每页大小
    /// </summary>
    /// <example>50</example>
    public int PageSize { get; set; } = 50;

    /// <summary>
    /// 偏移量
    /// </summary>
    public int OffSet => (PageIndex - 1) * PageSize;
}

/// <summary>
/// 分页出参
/// </summary>
public class PagedDto<T> : ListResultDto<T>
{
    public PagedDto(IReadOnlyList<T> items) : base(items)
    {
    }

    /// <summary>
    /// 总数
    /// </summary>
    public int Total { get; set; }
}
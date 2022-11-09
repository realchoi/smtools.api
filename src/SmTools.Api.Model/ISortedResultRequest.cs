namespace SmTools.Api.Model;

/// <summary>
/// 带有排序字段的接口
/// </summary>
public interface ISortedResultRequest
{
    /// <summary>
    /// 返回值的列排序方式，如 name asc，name desc，id desc 等
    /// </summary>
    string Sorting { get; set; }
}

namespace SmTools.Api.Model.BookmarkItems.Dtos;

/// <summary>
/// 书签条目 DTO
/// </summary>
public class BookmarkItemDto : IdInput<string>
{
    /// <summary>
    /// 书签名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 书签网址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 分类目录 id
    /// </summary>
    public string CategoryId { get; set; }
}
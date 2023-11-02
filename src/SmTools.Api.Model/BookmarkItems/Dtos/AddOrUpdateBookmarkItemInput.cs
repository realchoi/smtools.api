namespace SmTools.Api.Model.BookmarkItems.Dtos;

/// <summary>
/// 新增/编辑书签条目入参
/// </summary>
public class AddOrUpdateBookmarkItemInput : IdInput<string>
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
    /// 所属分类目录的 id，关联 bookmark_category 表的主键
    /// </summary>
    public string CategoryId { get; set; }

    /// <summary>
    /// 用户 Id，关联 public.user_info 表的主键
    /// </summary>
    public string UserId { get; set; }
}
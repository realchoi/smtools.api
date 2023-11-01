namespace SmTools.Api.Model.BookmarkItems.Dtos;

public class GetBookmarkItemListInput
{
    /// <summary>
    /// 用户 Id，关联 public.user_info 表的主键
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 书签分类目录 id
    /// </summary>
    public string CategoryId { get; set; }
}
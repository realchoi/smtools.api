namespace SmTools.Api.Model.BookmarkCategories.Dtos;

public class GetBookmarkCategoryTreeInput
{
    /// <summary>
    /// 用户 Id，关联 public.user_info 表的主键
    /// </summary>
    public string UserId { get; set; }
}
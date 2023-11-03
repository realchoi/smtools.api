namespace SmTools.Api.Model.BookmarkCategories.Dtos;

/// <summary>
/// 新增/编辑分类目录入参
/// </summary>
public class AddOrUpdateBookmarkCategoryInput : IdInput<string>
{
    /// <summary>
    /// 分类目录名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 父级 id
    /// </summary>
    public string ParentId { get; set; }

    /// <summary>
    /// 用户 Id，关联 public.user_info 表的主键
    /// </summary>
    public string UserId { get; set; }
}
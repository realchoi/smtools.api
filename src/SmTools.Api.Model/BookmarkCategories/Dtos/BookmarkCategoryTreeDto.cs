namespace SmTools.Api.Model.BookmarkCategories.Dtos;

/// <summary>
/// 书签分类目录树 DTO
/// </summary>
public class BookmarkCategoryTreeDto
{
    /// <summary>
    /// 树的 key 属性（对应：主键 ID）
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 树的 label 属性（对应：文件夹名称）
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 是否是叶子节点
    /// </summary>
    public bool IsLeaf { get; set; }
    
    /// <summary>
    /// 父级 id
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// 子集文件夹
    /// </summary>
    public List<BookmarkCategoryTreeDto> Children { get; set; }
}
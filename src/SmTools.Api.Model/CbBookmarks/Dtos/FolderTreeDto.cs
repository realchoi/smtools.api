namespace SmTools.Api.Model.CbBookmarks.Dtos;

/// <summary>
/// 书签文件夹树 DTO
/// </summary>
public class FolderTreeDto
{
    /// <summary>
    /// 主键 ID
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 文件夹名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 子集文件夹
    /// </summary>
    public List<FolderTreeDto> Children { get; set; }
}
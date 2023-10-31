namespace SmTools.Api.Model.CbBookmarks.Dtos;

public class GetFolderTreeInput
{
    /// <summary>
    /// 用户 Id，关联 public.user_info 表的主键
    /// </summary>
    public string UserId { get; set; }
}
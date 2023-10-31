using SmTools.Api.Model.CbBookmarks.Dtos;

namespace SmTools.Api.Application.CbBookmarks;

/// <summary>
/// 书签文件夹服务接口
/// </summary>
public interface IFolderAppService
{
    /// <summary>
    /// 获取用户的书签文件夹树
    /// </summary>
    /// <param name="userId">用户 id</param>
    /// <returns></returns>
    Task<List<FolderTreeDto>> GetFolderTree(string userId);
}
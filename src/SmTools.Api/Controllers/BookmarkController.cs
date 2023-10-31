using Microsoft.AspNetCore.Mvc;
using SmTools.Api.Application.CbBookmarks;
using SmTools.Api.Model.CbBookmarks.Dtos;

namespace SmTools.Api.Controllers;

/// <summary>
/// 书签管理器
/// </summary>
[ApiController]
[Route("bookmark")]
public class BookmarkController : ControllerBase
{
    private readonly IFolderAppService _folderAppService;

    public BookmarkController(IFolderAppService folderAppService)
    {
        _folderAppService = folderAppService;
    }

    /// <summary>
    /// 获取用户的书签文件夹树
    /// </summary>
    /// <param name="userId">用户 id</param>
    /// <returns></returns>
    [HttpGet("tree")]
    public async Task<List<FolderTreeDto>> GetFolderTree(string userId)
    {
        return await _folderAppService.GetFolderTree(userId);
    }
}
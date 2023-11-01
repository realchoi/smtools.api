using Microsoft.AspNetCore.Mvc;
using SmTools.Api.Application.BookmarkCategories;
using SmTools.Api.Model.BookmarkCategories.Dtos;

namespace SmTools.Api.Controllers;

/// <summary>
/// 书签管理器
/// </summary>
[ApiController]
[Route("bookmark")]
public class BookmarkController : ControllerBase
{
    private readonly IBookmarkCategoryAppService _bookmarkCategoryAppService;

    public BookmarkController(IBookmarkCategoryAppService bookmarkCategoryAppService)
    {
        _bookmarkCategoryAppService = bookmarkCategoryAppService;
    }

    /// <summary>
    /// 获取用户的书签文件夹树
    /// </summary>
    /// <param name="userId">用户 id</param>
    /// <returns></returns>
    [HttpGet("category/tree")]
    public async Task<List<BookmarkCategoryTreeDto>> GetBookmarkCategoryTree(string userId)
    {
        return await _bookmarkCategoryAppService.GetBookmarkCategoryTree(userId);
    }
}
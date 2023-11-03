using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmTools.Api.Application.BookmarkCategories;
using SmTools.Api.Application.BookmarkItems;
using SmTools.Api.Model.BookmarkCategories.Dtos;
using SmTools.Api.Model.BookmarkItems.Dtos;

namespace SmTools.Api.Controllers;

/// <summary>
/// 书签管理器
/// </summary>
[ApiController]
[Route("bookmark")]
[Authorize]
public class BookmarkController : ControllerBase
{
    private readonly IBookmarkCategoryAppService _bookmarkCategoryAppService;
    private readonly IBookmarkItemAppService _bookmarkItemAppService;

    public BookmarkController(IBookmarkCategoryAppService bookmarkCategoryAppService,
        IBookmarkItemAppService bookmarkItemAppService)
    {
        _bookmarkCategoryAppService = bookmarkCategoryAppService;
        _bookmarkItemAppService = bookmarkItemAppService;
    }

    /// <summary>
    /// 新建/编辑书签条目
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("category")]
    public async Task<string> AddOrUpdate(AddOrUpdateBookmarkCategoryInput input)
    {
        return await _bookmarkCategoryAppService.AddOrUpdate(input);
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

    /// <summary>
    /// 新建/编辑书签条目
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("item")]
    public async Task<string> AddOrUpdateBookmarkItem(AddOrUpdateBookmarkItemInput input)
    {
        return await _bookmarkItemAppService.AddOrUpdate(input);
    }

    /// <summary>
    /// 获取书签条目列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("item/list")]
    public async Task<List<BookmarkItemDto>> GetBookmarkItemList(GetBookmarkItemListInput input)
    {
        return await _bookmarkItemAppService.GetBookmarkItemList(input);
    }
}
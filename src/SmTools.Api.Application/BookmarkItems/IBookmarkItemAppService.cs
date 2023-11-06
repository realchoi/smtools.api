using SmTools.Api.Model.BookmarkItems.Dtos;

namespace SmTools.Api.Application.BookmarkItems;

/// <summary>
/// 书签条目服务接口
/// </summary>
public interface IBookmarkItemAppService
{
    /// <summary>
    /// 新建/编辑书签条目
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> AddOrUpdate(AddOrUpdateBookmarkItemInput input);

    /// <summary>
    /// 获取书签条目列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<BookmarkItemDto>> GetBookmarkItemList(GetBookmarkItemListInput input);

    /// <summary>
    /// 删除书签条目
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> Delete(string id, string userId);
}
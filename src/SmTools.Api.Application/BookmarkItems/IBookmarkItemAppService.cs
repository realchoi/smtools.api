using SmTools.Api.Model.BookmarkItems.Dtos;

namespace SmTools.Api.Application.BookmarkItems;

/// <summary>
/// 书签条目服务接口
/// </summary>
public interface IBookmarkItemAppService
{
    /// <summary>
    /// 获取书签条目列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<BookmarkItemDto>> GetBookmarkItemList(GetBookmarkItemListInput input);
}
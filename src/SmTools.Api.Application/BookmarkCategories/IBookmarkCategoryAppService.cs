using SmTools.Api.Model.BookmarkCategories.Dtos;

namespace SmTools.Api.Application.BookmarkCategories;

/// <summary>
/// 书签分类目录服务接口
/// </summary>
public interface IBookmarkCategoryAppService
{
    /// <summary>
    /// 获取用户的书签分类目录树
    /// </summary>
    /// <param name="userId">用户 id</param>
    /// <returns></returns>
    Task<List<BookmarkCategoryTreeDto>> GetBookmarkCategoryTree(string userId);
}
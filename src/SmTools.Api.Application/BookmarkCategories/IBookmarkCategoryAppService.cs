using SmTools.Api.Model.BookmarkCategories.Dtos;

namespace SmTools.Api.Application.BookmarkCategories;

/// <summary>
/// 书签分类目录服务接口
/// </summary>
public interface IBookmarkCategoryAppService
{
    /// <summary>
    /// 新建/编辑书签条目
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> AddOrUpdate(AddOrUpdateBookmarkCategoryInput input);

    /// <summary>
    /// 获取用户的书签分类目录树
    /// </summary>
    /// <param name="userId">用户 id</param>
    /// <returns></returns>
    Task<List<BookmarkCategoryTreeDto>> GetBookmarkCategoryTree(string userId);

    /// <summary>
    /// 删除分类目录
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> Delete(string id, string userId);
}
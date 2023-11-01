using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.BookmarkCategories;
using SmTools.Api.Model.BookmarkCategories.Dtos;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;
using SpringMountain.Framework.Domain.Repositories;

namespace SmTools.Api.Application.BookmarkCategories;

/// <summary>
/// 书签分类目录服务实现类
/// </summary>
public class BookmarkCategoryAppService : IBookmarkCategoryAppService
{
    private readonly IRepository<BookmarkCategory, long> _bookmarkCategoryRepository;

    public BookmarkCategoryAppService(IRepository<BookmarkCategory, long> bookmarkCategoryRepository)
    {
        _bookmarkCategoryRepository = bookmarkCategoryRepository;
    }

    /// <summary>
    /// 获取用户的书签分类目录树
    /// </summary>
    /// <param name="userId">用户 id</param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<List<BookmarkCategoryTreeDto>> GetBookmarkCategoryTree(string userId)
    {
        if (userId.IsNullOrEmpty())
        {
            throw new InvalidParameterException("用户 id 不能为空");
        }

        if (!long.TryParse(userId, out var userIdValue))
        {
            throw new InvalidParameterException("用户 id 不正确");
        }

        var folders = await _bookmarkCategoryRepository.GetQueryable()
            .Where(p => p.UserId == userIdValue)
            .AsNoTracking().ToListAsync();
        var tree = RenderFolderTree(null);
        return tree;

        List<BookmarkCategoryTreeDto> RenderFolderTree(long? parentId = null)
        {
            return folders.Where(p => p.ParentId == parentId)
                .Select(c =>
                {
                    var children = RenderFolderTree(c.Id);
                    var item = new BookmarkCategoryTreeDto
                    {
                        Key = c.Id.ToString(),
                        Label = c.Name,
                        IsLeaf = !children.Any(),
                        Children = children
                    };
                    return item;
                }).ToList();
        }
    }
}
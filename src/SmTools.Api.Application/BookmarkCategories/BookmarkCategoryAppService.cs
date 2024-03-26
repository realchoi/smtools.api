using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.BookmarkCategories;
using SmTools.Api.Core.BookmarkItems;
using SmTools.Api.Model.BookmarkCategories.Dtos;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;
using SpringMountain.Framework.Domain.Repositories;
using SpringMountain.Framework.Snowflake;

namespace SmTools.Api.Application.BookmarkCategories;

/// <summary>
/// 书签分类目录服务实现类
/// </summary>
public class BookmarkCategoryAppService : IBookmarkCategoryAppService
{
    private readonly IRepository<BookmarkCategory, long> _bookmarkCategoryRepository;
    private readonly IRepository<BookmarkItem, long> _bookmarkItemRepository;

    public BookmarkCategoryAppService(IRepository<BookmarkCategory, long> bookmarkCategoryRepository,
        IRepository<BookmarkItem, long> bookmarkItemRepository)
    {
        _bookmarkCategoryRepository = bookmarkCategoryRepository;
        _bookmarkItemRepository = bookmarkItemRepository;
    }

    /// <summary>
    /// 新建/编辑书签条目
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<string> AddOrUpdate(AddOrUpdateBookmarkCategoryInput input)
    {
        if (input.UserId.IsNullOrEmpty())
        {
            throw new InvalidParameterException("用户 id 不能为空");
        }

        if (!long.TryParse(input.UserId, out var userId))
        {
            throw new InvalidParameterException("用户 id 不正确");
        }

        if (input.ParentId.IsNullOrEmpty())
        {
            throw new InvalidParameterException("父级目录 id 不能为空");
        }

        if (!long.TryParse(input.ParentId, out var parentId))
        {
            throw new InvalidParameterException("父级目录 id 不正确");
        }

        long id = 0;
        if (!input.Id.IsNullOrEmpty())
        {
            if (!long.TryParse(input.Id, out id))
                throw new InvalidParameterException("分类目录 id 不正确");
        }

        var existName = await _bookmarkCategoryRepository.GetQueryable()
            .AnyAsync(p => p.UserId == userId && p.ParentId == parentId && p.Name == input.Name && p.Id != id);
        if (existName)
        {
            throw new InvalidParameterException("相同层级下已有同名文件夹");
        }

        var entity = await _bookmarkCategoryRepository.GetQueryable()
            .Where(p => p.UserId == userId && p.ParentId == parentId && p.Id == id)
            .FirstOrDefaultAsync();
        // 新增
        if (entity == null)
        {
            var newId = IdGenerator.NextId();
            entity = new BookmarkCategory(newId, input.Name, userId, parentId);
            await _bookmarkCategoryRepository.AddAsync(entity);
        }
        // 编辑
        else
        {
            entity.Name = input.Name;
            entity.ModificationTime = DateTime.Now;
        }

        return entity.Id.ToString();
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

        var bookmarkCategories = await _bookmarkCategoryRepository.GetQueryable()
            .Where(p => p.UserId == userIdValue)
            .AsNoTracking().ToListAsync();
        var tree = RenderFolderTree(null);
        return tree;

        List<BookmarkCategoryTreeDto> RenderFolderTree(long? parentId = null)
        {
            return bookmarkCategories.Where(p => p.ParentId == parentId)
                .Select(c =>
                {
                    var children = RenderFolderTree(c.Id);
                    var item = new BookmarkCategoryTreeDto
                    {
                        Key = c.Id.ToString(),
                        Label = c.Name,
                        IsLeaf = !children.Any(),
                        ParentId = parentId?.ToString(),
                        Children = children
                    };
                    return item;
                }).ToList();
        }
    }

    /// <summary>
    /// 删除分类目录
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<bool> Delete(string id, string userId)
    {
        if (id.IsNullOrEmpty())
        {
            throw new InvalidParameterException("分类目录 id 不能为空");
        }

        if (!long.TryParse(id, out var idValue))
        {
            throw new InvalidParameterException("分类目录 id 不正确");
        }

        if (userId.IsNullOrEmpty())
        {
            throw new InvalidParameterException("用户 id 不能为空");
        }

        if (!long.TryParse(userId, out var userIdValue))
        {
            throw new InvalidParameterException("用户 id 不正确");
        }

        var entity = await _bookmarkCategoryRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.UserId == userIdValue && p.Id == idValue);
        if (entity == null)
        {
            throw new InvalidParameterException("分类目录 id 不存在");
        }

        if (entity.ParentId == null)
        {
            throw new InvalidParameterException("顶级文件夹不能删除");
        }

        // 当前用户的所有分类目录
        var allUserCategories = await _bookmarkCategoryRepository.GetQueryable()
            .Where(p => p.UserId == userIdValue)
            .Select(c => new { c.Id, c.ParentId })
            .AsNoTracking().ToListAsync();

        await _bookmarkCategoryRepository.RemoveAsync(entity);
        // 删除所有子孙节点
        var allChildrenIds = GetChildrenIds(entity.Id);
        var allChildren = _bookmarkCategoryRepository.GetQueryable()
            .Where(p => p.UserId == userIdValue && allChildrenIds.Contains(p.Id));
        await _bookmarkCategoryRepository.RemoveAsync(allChildren);

        // 删除所有书签条目
        foreach (var categoryId in allChildrenIds.Concat(new List<long> { entity.Id }))
        {
            var bookmarkItems = _bookmarkItemRepository.GetQueryable()
                .Where(p => p.UserId == userIdValue && p.CategoryId == categoryId);
            await _bookmarkItemRepository.RemoveAsync(bookmarkItems);
        }

        return true;

        List<long> GetChildrenIds(long? parentId = null)
        {
            var result = new List<long>();
            var childrenIds = allUserCategories.Where(p => p.ParentId == parentId).Select(c => c.Id).ToList();
            result.AddRange(childrenIds);
            foreach (var childrenId in childrenIds)
            {
                result.AddRange(GetChildrenIds(childrenId));
            }

            return result;
        }
    }
}

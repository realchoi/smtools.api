using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.BookmarkCategories;
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
    private readonly ISnowflakeIdMaker _snowflakeIdMaker;

    public BookmarkCategoryAppService(IRepository<BookmarkCategory, long> bookmarkCategoryRepository,
        ISnowflakeIdMaker snowflakeIdMaker)
    {
        _bookmarkCategoryRepository = bookmarkCategoryRepository;
        _snowflakeIdMaker = snowflakeIdMaker;
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
            var newId = _snowflakeIdMaker.NextId();
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
                        Children = children
                    };
                    return item;
                }).ToList();
        }
    }
}
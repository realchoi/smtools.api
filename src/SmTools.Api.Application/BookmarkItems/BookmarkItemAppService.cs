using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.BookmarkItems;
using SmTools.Api.Model.BookmarkItems.Dtos;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;
using SpringMountain.Framework.Domain.Repositories;
using SpringMountain.Framework.Snowflake;

namespace SmTools.Api.Application.BookmarkItems;

/// <summary>
/// 书签条目服务实现类
/// </summary>
public class BookmarkItemAppService : IBookmarkItemAppService
{
    private readonly IRepository<BookmarkItem, long> _bookmarkItemRepository;
    private readonly ISnowflakeIdMaker _snowflakeIdMaker;

    public BookmarkItemAppService(IRepository<BookmarkItem, long> bookmarkItemRepository,
        ISnowflakeIdMaker snowflakeIdMaker)
    {
        _bookmarkItemRepository = bookmarkItemRepository;
        _snowflakeIdMaker = snowflakeIdMaker;
    }

    /// <summary>
    /// 新建/编辑书签条目
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<string> AddOrUpdate(AddOrUpdateBookmarkItemInput input)
    {
        if (input.UserId.IsNullOrEmpty())
        {
            throw new InvalidParameterException("用户 id 不能为空");
        }

        if (!long.TryParse(input.UserId, out var userId))
        {
            throw new InvalidParameterException("用户 id 不正确");
        }

        if (input.CategoryId.IsNullOrEmpty())
        {
            throw new InvalidParameterException("分类目录 id 不能为空");
        }

        if (!long.TryParse(input.CategoryId, out var categoryId))
        {
            throw new InvalidParameterException("分类目录 id 不正确");
        }

        long id = 0;
        if (!input.Id.IsNullOrEmpty())
        {
            if (!long.TryParse(input.Id, out id))
                throw new InvalidParameterException("书签条目 id 不正确");
        }

        var entity = await _bookmarkItemRepository.GetQueryable()
            .Where(p => p.UserId == userId && p.CategoryId == categoryId && p.Id == id)
            .FirstOrDefaultAsync();
        // 新增
        if (entity == null)
        {
            var newId = _snowflakeIdMaker.NextId();
            entity = new BookmarkItem(newId, input.Name, input.Url, userId, categoryId);
            await _bookmarkItemRepository.AddAsync(entity);
        }
        // 编辑
        else
        {
            entity.Name = input.Name;
            entity.Url = input.Url;
            entity.ModificationTime = DateTime.Now;
        }

        return entity.Id.ToString();
    }

    /// <summary>
    /// 获取书签条目列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<List<BookmarkItemDto>> GetBookmarkItemList(GetBookmarkItemListInput input)
    {
        if (input.UserId.IsNullOrEmpty())
        {
            throw new InvalidParameterException("用户 id 不能为空");
        }

        if (!long.TryParse(input.UserId, out var userId))
        {
            throw new InvalidParameterException("用户 id 不正确");
        }

        Expression<Func<BookmarkItem, bool>> predict = item => item.UserId == userId;
        if (!input.CategoryId.IsNullOrEmpty())
        {
            if (!long.TryParse(input.CategoryId, out var categoryId))
            {
                throw new InvalidParameterException("分类目录 id 不正确");
            }

            predict = predict.And(item => item.CategoryId == categoryId);
        }

        var bookmarkItems = await _bookmarkItemRepository.GetQueryable()
            .Where(predict)
            .AsNoTracking().ToListAsync();
        var result = bookmarkItems.Select(c => new BookmarkItemDto
        {
            Id = c.Id.ToString(),
            Name = c.Name,
            Url = c.Url,
            CategoryId = c.CategoryId.ToString()
        }).ToList();
        return result;
    }

    /// <summary>
    /// 删除书签条目
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

        var entity = await _bookmarkItemRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.UserId == userIdValue && p.Id == idValue);
        if (entity == null)
        {
            throw new InvalidParameterException("书签条目 id 不存在");
        }

        await _bookmarkItemRepository.RemoveAsync(entity);
        return true;
    }
}
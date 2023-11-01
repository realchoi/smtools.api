using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.BookmarkItems;
using SmTools.Api.Model.BookmarkItems.Dtos;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;
using SpringMountain.Framework.Domain.Repositories;

namespace SmTools.Api.Application.BookmarkItems;

/// <summary>
/// 书签条目服务实现类
/// </summary>
public class BookmarkItemAppService : IBookmarkItemAppService
{
    private readonly IRepository<BookmarkItem, long> _bookmarkItemRepository;

    public BookmarkItemAppService(IRepository<BookmarkItem, long> bookmarkItemRepository)
    {
        _bookmarkItemRepository = bookmarkItemRepository;
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
}
using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.AiFavorites;
using SmTools.Api.Core.AiWebsites;
using SmTools.Api.Model;
using SmTools.Api.Model.AiFavorites.Dtos;
using SmTools.Api.Model.AiWebsites.Dtos;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;
using SpringMountain.Framework.Domain.Repositories;
using SpringMountain.Framework.Snowflake;

namespace SmTools.Api.Application.AiFavorites;

/// <summary>
/// AI 网站收藏服务实现类
/// </summary>
public class AiFavoriteAppService : IAiFavoriteAppService
{
    private readonly IRepository<AiFavorite, long> _aiFavoriteRepository;
    private readonly IRepository<AiWebsite, long> _aiWebsiteRepository;

    public AiFavoriteAppService(IRepository<AiFavorite, long> aiFavoriteRepository,
        IRepository<AiWebsite, long> aiWebsiteRepository)
    {
        _aiFavoriteRepository = aiFavoriteRepository;
        _aiWebsiteRepository = aiWebsiteRepository;
    }

    /// <summary>
    /// 新建/编辑 AI 网站收藏
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<string> AddOrUpdate(AddOrUpdateAiFavoriteInput input)
    {
        input.Validate();

        long id = 0;
        if (!input.Id.IsNullOrEmpty())
        {
            if (!long.TryParse(input.Id, out id))
                throw new InvalidParameterException("网站收藏 ID 不正确");
        }

        long userId = 0L;
        if (!input.UserId.IsNullOrEmpty())
        {
            if (!long.TryParse(input.UserId, out userId))
                throw new InvalidParameterException("用户 ID 不正确");
        }

        long websiteId = 0L;
        if (!input.WebsiteId.IsNullOrEmpty())
        {
            if (!long.TryParse(input.WebsiteId, out websiteId))
                throw new InvalidParameterException("AI 网站 ID 不正确");
        }

        var entity = await _aiFavoriteRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.Id == id);
        // 新增
        if (entity == null)
        {
            var newId = IdGenerator.NextId();
            entity = new AiFavorite(newId, userId, websiteId);
            await _aiFavoriteRepository.AddAsync(entity);
        }
        // 编辑
        else
        {
            entity.UserId = userId;
            entity.WebsiteId = websiteId;
            entity.ModificationTime = DateTime.Now;
        }

        return entity.Id.ToString();
    }

    /// <summary>
    /// 分页查询 AI 网站收藏
    /// </summary>
    public async Task<PagedDto<AiWebsiteDto>> QueryPageAsync(QueryAiFavoritePageInput input)
    {
        var categoryId = 0L;
        var userId = 0L;

        if (!input.CategoryId.IsNullOrEmpty())
        {
            if (!long.TryParse(input.CategoryId, out categoryId))
            {
                throw new InvalidParameterException("分类目录 ID 不正确");
            }
        }

        if (!input.UserId.IsNullOrEmpty())
        {
            if (!long.TryParse(input.UserId, out userId))
            {
                throw new InvalidParameterException("用户 ID 不正确");
            }
        }

        // 构建查询
        var query =
            from favorite in _aiFavoriteRepository.GetQueryable()
            join website in _aiWebsiteRepository.GetQueryable()
                on favorite.WebsiteId equals website.Id
            where favorite.UserId == userId
            select new { favorite, website };

        // 分类筛选
        if (categoryId > 0)
        {
            query = query.Where(x => x.website.CategoryId == categoryId);
        }

        // 关键词搜索
        if (!string.IsNullOrWhiteSpace(input.Keyword))
        {
            input.Keyword = input.Keyword.Trim().ToLower();
            query = query.Where(x =>
                x.website.Name.ToLower().Contains(input.Keyword) ||
                x.website.Description.ToLower().Contains(input.Keyword) ||
                x.website.Tags.Contains(input.Keyword));
        }

        // 获取总数
        var total = await query.CountAsync();

        // 分页查询
        var items = await query
            .OrderByDescending(x => x.favorite.CreationTime)
            .Skip(input.OffSet).Take(input.PageSize)
            .Select(x => new AiWebsiteDto
            {
                Id = x.website.Id.ToString(),
                Name = x.website.Name,
                Description = x.website.Description,
                CategoryId = x.website.CategoryId.ToString(),
                Url = x.website.Url,
                Logo = x.website.Logo,
                Tags = x.website.Tags,
                CreationTime = x.website.CreationTime,
                ModificationTime = x.website.ModificationTime
            })
            .ToListAsync();

        return new PagedDto<AiWebsiteDto>(items, total);
    }
}
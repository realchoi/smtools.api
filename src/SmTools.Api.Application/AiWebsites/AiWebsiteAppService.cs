using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.AiWebsites;
using SmTools.Api.Model;
using SmTools.Api.Model.AiWebsites.Dtos;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;
using SpringMountain.Framework.Domain.Repositories;
using SpringMountain.Framework.Snowflake;
using SpringMountain.Framework.Uow;

namespace SmTools.Api.Application.AiWebsites;

/// <summary>
/// AI 网站服务实现类
/// </summary>
public class AiWebsiteAppService : IAiWebsiteAppService
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IRepository<AiWebsite, long> _aiWebsiteRepository;

    public AiWebsiteAppService(IUnitOfWorkManager unitOfWorkManager,
        IRepository<AiWebsite, long> aiWebsiteRepository)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _aiWebsiteRepository = aiWebsiteRepository;
    }

    /// <summary>
    /// 新建/编辑 AI 网站
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<string> AddOrUpdate(AddOrUpdateAiWebsiteInput input)
    {
        if (input.CategoryId.IsNullOrWhiteSpace())
        {
            throw new InvalidParameterException("分类 ID 不能为空");
        }

        if (!long.TryParse(input.CategoryId, out var categoryId))
        {
            throw new InvalidParameterException("分类 ID 不正确");
        }

        long id = 0;
        if (!input.Id.IsNullOrEmpty())
        {
            if (!long.TryParse(input.Id, out id))
                throw new InvalidParameterException("网站 ID 不正确");
        }

        var entity = await _aiWebsiteRepository.GetQueryable()
            .Where(p => p.CategoryId == categoryId && p.Id == id)
            .FirstOrDefaultAsync();
        // 新增
        if (entity == null)
        {
            var newId = IdGenerator.NextId();
            entity = new AiWebsite(newId, input.Name, input.Description, categoryId, input.Url, input.Logo, input.Tags);
            await _aiWebsiteRepository.AddAsync(entity);
        }
        // 编辑
        else
        {
            entity.Name = input.Name;
            entity.Description = input.Description;
            entity.CategoryId = categoryId;
            entity.Url = input.Url;
            entity.Logo = input.Logo;
            entity.Tags = input.Tags;
            entity.ModificationTime = DateTime.Now;
        }

        return entity.Id.ToString();
    }

    /// <summary>
    /// 分页查询 AI 网站
    /// </summary>
    public async Task<PagedDto<AiWebsiteDto>> QueryPageAsync(QueryAiWebsitePageInput input)
    {
        var categoryId = 0L;

        if (!input.CategoryId.IsNullOrEmpty())
        {
            if (!long.TryParse(input.CategoryId, out categoryId))
            {
                throw new InvalidParameterException("分类目录 ID 不正确");
            }
        }

        var query = _aiWebsiteRepository.GetQueryable();

        // 关键词搜索
        if (!string.IsNullOrWhiteSpace(input.Keyword))
        {
            input.Keyword = input.Keyword.Trim();

            query = query.WhereIf(categoryId != 0, x => x.CategoryId == categoryId)
                .Where(x =>
                    x.Name.ToLower().Contains(input.Keyword.ToLower()) ||
                    x.Description.ToLower().Contains(input.Keyword.ToLower()) ||
                    x.Tags.Contains(input.Keyword));
        }

        // 获取总数
        var total = await query.CountAsync();

        // 分页查询
        var items = await query
            .OrderByDescending(x => x.CreationTime)
            .Skip(input.OffSet).Take(input.PageSize)
            .Select(x => new AiWebsiteDto
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Description = x.Description,
                CategoryId = x.CategoryId.ToString(),
                Url = x.Url,
                Logo = x.Logo,
                Tags = x.Tags,
                CreationTime = x.CreationTime,
                ModificationTime = x.ModificationTime
            })
            .ToListAsync();

        return new PagedDto<AiWebsiteDto>(items, total);
    }
}
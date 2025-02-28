using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.AiCategories;
using SmTools.Api.Model;
using SmTools.Api.Model.AiCategories.Dtos;
using SpringMountain.Api.Exceptions.Contracts.Exceptions.Request;
using SpringMountain.Framework.Domain.Repositories;
using SpringMountain.Framework.Snowflake;

namespace SmTools.Api.Application.AiCategories;

/// <summary>
/// AI 网站分类服务实现类
/// </summary>
public class AiCategoryAppService : IAiCategoryAppService
{
    private readonly IRepository<AiCategory, long> _aiCategoryRepository;

    public AiCategoryAppService(IRepository<AiCategory, long> aiCategoryRepository)
    {
        _aiCategoryRepository = aiCategoryRepository;
    }

    /// <summary>
    /// 新建/编辑 AI 网站分类
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    public async Task<string> AddOrUpdate(AddOrUpdateAiCategoryInput input)
    {
        input.Validate();

        long id = 0;
        if (!input.Id.IsNullOrEmpty())
        {
            if (!long.TryParse(input.Id, out id))
                throw new InvalidParameterException("网站分类 ID 不正确");
        }

        var entity = await _aiCategoryRepository.GetQueryable()
            .FirstOrDefaultAsync(p => p.Id == id);
        // 新增
        if (entity == null)
        {
            var newId = IdGenerator.NextId();
            entity = new AiCategory(newId, input.Name);
            await _aiCategoryRepository.AddAsync(entity);
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
    /// 分页查询 AI 网站分类
    /// </summary>
    public async Task<PagedDto<AiCategoryDto>> QueryPageAsync(QueryAiCategoryPageInput input)
    {
        var query = _aiCategoryRepository.GetQueryable();

        // 关键词搜索
        if (!string.IsNullOrWhiteSpace(input.Keyword))
        {
            input.Keyword = input.Keyword.Trim();

            query = query.Where(x => x.Name.ToLower().Contains(input.Keyword.ToLower()));
        }

        // 获取总数
        var total = await query.CountAsync();

        // 分页查询
        var items = await query
            .OrderByDescending(x => x.CreationTime)
            .Skip(input.OffSet).Take(input.PageSize)
            .Select(x => new AiCategoryDto
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                CreationTime = x.CreationTime,
                ModificationTime = x.ModificationTime
            })
            .ToListAsync();

        return new PagedDto<AiCategoryDto>(items, total);
    }
}
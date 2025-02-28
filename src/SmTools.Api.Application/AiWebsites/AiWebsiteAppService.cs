using Microsoft.EntityFrameworkCore;
using SmTools.Api.Core.AiWebsites;
using SmTools.Api.Model;
using SmTools.Api.Model.AiWebsites.Dtos;
using SpringMountain.Framework.Domain.Repositories;
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
    /// 分页查询 AI 网站
    /// </summary>
    public async Task<PagedDto<AiWebsiteDto>> QueryPageAsync(QueryAiWebsitePageInput input)
    {
        var query = _aiWebsiteRepository.GetQueryable();

        // 关键词搜索
        if (!string.IsNullOrWhiteSpace(input.Keyword))
        {
            input.Keyword = input.Keyword.Trim();

            query = query.Where(x =>
                x.Name.ToLower().Contains(input.Keyword.ToLower()) ||
                x.Description.ToLower().Contains(input.Keyword.ToLower()) ||
                x.Category.Contains(input.Keyword) ||
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
                Category = x.Category,
                Url = x.Url,
                Logo = x.Logo,
                Tags = x.Tags,
                CreationTime = x.CreationTime
            })
            .ToListAsync();

        return new PagedDto<AiWebsiteDto>(items)
        {
            Total = total
        };
    }
}
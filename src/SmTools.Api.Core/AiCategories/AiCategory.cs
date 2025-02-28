using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.AiCategories;

/// <summary>
/// AI 网站分类
/// </summary>
[Table("ai_category")]
[Comment("AI 网站分类表")]
public class AiCategory : Entity<long>, IHasTimeAuditing
{
    /// <summary>
    /// 创建 AI 网站分类
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public AiCategory(long id, string name)
    {
        this.Id = id;
        this.Name = name;
        this.CreationTime = DateTime.Now;
        this.ModificationTime = DateTime.Now;
    }

    /// <summary>
    /// 分类名称
    /// </summary>
    [Comment("分类名称")]
    public string Name { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Comment("创建时间")]
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Comment("修改时间")]
    public DateTime? ModificationTime { get; set; }
}
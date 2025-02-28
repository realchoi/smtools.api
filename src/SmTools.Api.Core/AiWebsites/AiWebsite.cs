using Microsoft.EntityFrameworkCore;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.AiWebsites;

/// <summary>
/// AI 网站表
/// </summary>
[Comment("AI 网站表")]
public class AiWebsite : Entity<long>, IHasTimeAuditing
{
    /// <summary>
    /// 创建 AI 网站
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="url"></param>
    /// <param name="userId"></param>
    /// <param name="categoryId"></param>
    public AiWebsite(long id, string name, string description,
        long categoryId, string url, string logo, List<string> tags)
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;
        this.CategoryId = categoryId;
        this.Url = url;
        this.Logo = logo;
        this.Tags = tags;
        this.CreationTime = DateTime.Now;
        this.ModificationTime = DateTime.Now;
    }

    /// <summary>
    /// 名称
    /// </summary>
    [Comment("名称")]
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Comment("描述")]
    public string Description { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    [Comment("分类 Id，关联 website_category 表的主键")]
    public long CategoryId { get; set; }

    /// <summary>
    /// 网站地址
    /// </summary>
    [Comment("网站地址")]
    public string Url { get; set; }

    /// <summary>
    /// 网站 Logo
    /// </summary>
    [Comment("网站 Logo")]
    public string Logo { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    [Comment("标签")]
    public List<string> Tags { get; set; }

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
using Microsoft.EntityFrameworkCore;
using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.Systems;

/// <summary>
/// 权限表
/// </summary>
[Comment("权限表")]
public class Permission : Entity<long>, IHasTimeAndUserAuditing
{
    /// <summary>
    /// 名称
    /// </summary>
    [Comment("名称")]
    public string Name { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    [Comment("代码")]
    public string Code { get; set; }

    /// <summary>
    /// 前台 url
    /// </summary>
    [Comment("前台 url")]
    public string Url { get; set; }

    /// <summary>
    /// 标识
    /// </summary>
    [Comment("标识")]
    public string Perms { get; set; }

    /// <summary>
    /// 类型；0:目录 1:菜单 2:按钮
    /// </summary>
    [Comment("类型；0:目录 1:菜单 2:按钮")]
    public string Type { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [Comment("图标")]
    public string Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Comment("排序")]
    public int? OrderNum { get; set; }

    /// <summary>
    /// 权限 api
    /// </summary>
    [Comment("权限 api")]
    public string ApiUrl { get; set; }

    /// <summary>
    /// 内嵌的 iframe 路径
    /// </summary>
    [Comment("内嵌的 iframe 路径")]
    public string IframeUrl { get; set; }

    /// <summary>
    /// 父级 id
    /// </summary>
    [Comment("父级 id")]
    public long ParentId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Comment("备注")]
    public string Remark { get; set; }

    // /// <summary>
    // /// 子权限列表
    // /// </summary>
    // public List<Permission> Child { get; set; }

    /// <summary>
    /// 创建用户
    /// </summary>
    [Comment("创建用户")]
    public string CreationUser { get; set; }

    /// <summary>
    /// 修改用户
    /// </summary>
    [Comment("修改用户")]
    public string? ModificationUser { get; set; }

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
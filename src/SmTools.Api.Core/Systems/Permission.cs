using SpringMountain.Framework.Domain.Auditing;
using SpringMountain.Framework.Domain.Entities;

namespace SmTools.Api.Core.Systems;

/// <summary>
/// 权限表
/// </summary>
public class Permission : Entity<long>, IHasTimeAndUserAuditing
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 前台 url
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 标识
    /// </summary>
    public string Perms { get; set; }

    /// <summary>
    /// 类型；0:目录 1:菜单 2:按钮
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? OrderNum { get; set; }

    /// <summary>
    /// 权限 api
    /// </summary>
    public string ApiUrl { get; set; }

    /// <summary>
    /// 内嵌的 iframe 路径
    /// </summary>
    public string IframeUrl { get; set; }

    /// <summary>
    /// 父级 id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    // /// <summary>
    // /// 子权限列表
    // /// </summary>
    // public List<Permission> Child { get; set; }

    /// <summary>
    /// 创建用户
    /// </summary>
    public string CreationUser { get; set; }

    /// <summary>
    /// 修改用户
    /// </summary>
    public string? ModificationUser { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModificationTime { get; set; }
}
using Microsoft.EntityFrameworkCore;

namespace SpringMountain.Framework.Domain.Auditing;

public interface IHasCreationUser
{
    /// <summary>
    /// 创建用户
    /// </summary>
    [Comment("创建用户")]
    public string CreationUser { get; set; }
}

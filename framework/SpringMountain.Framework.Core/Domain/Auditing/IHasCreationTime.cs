using Microsoft.EntityFrameworkCore;

namespace SpringMountain.Framework.Domain.Auditing;

public interface IHasCreationTime
{
    /// <summary>
    /// 创建时间
    /// </summary>
    [Comment("创建时间")]
    DateTime CreationTime { get; set; }
}

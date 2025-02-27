using Microsoft.EntityFrameworkCore;

namespace SpringMountain.Framework.Domain.Auditing;

public interface IHasModificationTime
{
    /// <summary>
    /// 修改时间
    /// </summary>
    [Comment("修改时间")]
    DateTime? ModificationTime { get; set; }
}

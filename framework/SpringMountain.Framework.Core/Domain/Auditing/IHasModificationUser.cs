using Microsoft.EntityFrameworkCore;

namespace SpringMountain.Framework.Domain.Auditing;

public interface IHasModificationUser
{
    /// <summary>
    /// 修改用户
    /// </summary>
    [Comment("修改用户")]
    public string ModificationUser { get; set; }
}

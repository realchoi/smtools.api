using Microsoft.EntityFrameworkCore;

namespace SpringMountain.Framework.Domain.Entities;

/// <summary>
/// 实体接口
/// </summary>
public interface IEntity
{
}

/// <summary>
/// 实体接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntity<TKey> : IEntity
{
    [Comment("主键")]
    TKey Id { get; set; }
}

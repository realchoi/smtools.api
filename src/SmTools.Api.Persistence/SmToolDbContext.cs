using Microsoft.EntityFrameworkCore;
using SpringMountain.Framework.Domain.Repositories;

namespace SmTools.Api.Persistence;

/// <summary>
/// 数据库上下文
/// </summary>
public class SmToolDbContext : CoreDbContext
{
    public SmToolDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        base.OnModelCreating(modelBuilder);
    }
}

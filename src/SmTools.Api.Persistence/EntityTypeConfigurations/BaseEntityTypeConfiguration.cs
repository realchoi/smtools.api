using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpringMountain.Framework.Domain.Entities;
using SpringMountain.Framework.Snowflake;

namespace SmTools.Api.Persistence.EntityTypeConfigurations;

public class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity<long>
{
    private readonly SnowflakeIdMaker _snowflakeIdMaker;

    public BaseEntityTypeConfiguration(SnowflakeIdMaker snowflakeIdMaker)
    {
        _snowflakeIdMaker = snowflakeIdMaker;
    }

    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasValueGenerator((a, b) => new IdGenerator(_snowflakeIdMaker)).ValueGeneratedOnAdd();
        builder.Property(t => t.Id).HasColumnName("Id");
    }
}

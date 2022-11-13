using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SpringMountain.Framework.Snowflake;

namespace SmTools.Api.Persistence.EntityTypeConfigurations;

public class IdGenerator : ValueGenerator<long>
{
    private readonly SnowflakeIdMaker _snowflakeIdMaker;

    public override bool GeneratesTemporaryValues => false;

    public IdGenerator(SnowflakeIdMaker snowflakeIdMaker)
    {
        _snowflakeIdMaker = snowflakeIdMaker;
    }

    public override long Next(EntityEntry entry)
    {
        return _snowflakeIdMaker.NextId();
    }
}

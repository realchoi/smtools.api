using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SpringMountain.Modularity;
using SpringMountain.Framework.Uow.EntityFrameworkCore;

namespace SpringMountain.Framework.Uow;

public class UowModule : CoreModuleBase
{
    /// <summary>
    /// 配置服务容器
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceCollectionContext context)
    {
        var services = context.Services;
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IUnitOfWorkManager, UnitOfWorkManager>();
        services.AddSingleton<IAmbientUnitOfWork, AmbientUnitOfWork>();
        context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
        base.ConfigureServices(context);
    }
}

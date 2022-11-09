using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmTools.Api.Persistence;
using SpringMountain.Modularity;
using SpringMountain.Modularity.Attribute;

namespace SmTools.Api.Application;

/// <summary>
/// Application 模块
/// </summary>
[DependsOn(typeof(PersistenceModule))]
public class ApplicationModule : CoreModuleBase
{
    /// <summary>
    /// 配置服务容器
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceCollectionContext context)
    {
        var services = context.Services;
        AddApplicationServices(services);
    }

    /// <summary>
    /// 将 Application 模块下的所有 Service 类注入到容器中。
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddApplicationServices(IServiceCollection services)
    {
        var assembly = typeof(ApplicationModule).Assembly;
        var types = assembly
            .GetTypes()
            .Where(p =>
                !p.IsGenericType &&
                !p.IsAbstract &&
                p.IsClass &&
                p.Name.EndsWith("Service"))
            .ToList();

        foreach (var type in types)
        {
            var baseType = type.GetInterfaces().FirstOrDefault(p => p.Name == $"I{type.Name}");
            if (baseType != null)
            {
                services.TryAddScoped(baseType, type);
            }
        }
        return services;
    }
}

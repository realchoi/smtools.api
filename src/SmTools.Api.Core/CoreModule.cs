using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SpringMountain.Framework.Uow;
using SpringMountain.Modularity;
using SpringMountain.Modularity.Attribute;

namespace SmTools.Api.Core;

[DependsOn(typeof(UowModule))]
public class CoreModule : CoreModuleBase
{
    /// <summary>
    /// 配置服务容器
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceCollectionContext context)
    {
        var services = context.Services;
        AddServices(services);
    }

    /// <summary>
    /// 将 Core 模块下的所有 Service 类注入到容器中。
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddServices(IServiceCollection services)
    {
        var assembly = typeof(CoreModule).Assembly;
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

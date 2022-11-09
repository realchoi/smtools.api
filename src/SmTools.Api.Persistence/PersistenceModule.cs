using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmTools.Api.Core;
using SpringMountain.Modularity;
using SpringMountain.Modularity.Attribute;

namespace SmTools.Api.Persistence;

/// <summary>
/// 数据库持久化模块
/// </summary>
[DependsOn(typeof(CoreModule))]
public class PersistenceModule : CoreModuleBase
{
    public IConfiguration Configuration { get; set; }

    public PersistenceModule(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// 配置服务容器
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceCollectionContext context)
    {
        var services = context.Services;

        /*services.AddDbContext<SmToolDbContext>(options =>
        {
            options.UseNpgsql(Configuration["Db:SmTool:ConnectionString"]);
        });

        services.AddRepositories<SmToolDbContext>();*/
        AddRepositories(services);
    }

    /// <summary>
    /// 配置中间件
    /// </summary>
    /// <param name="context"></param>
    public override void Configure(ApplicationBuilderContext context)
    {
        /*var serviceProvider = context.ApplicationBuilder.ApplicationServices;
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SmToolDbContext>();
        // 数据库迁移在此执行
        dbContext.Database.Migrate();*/
    }

    /// <summary>
    /// 将 Persistence 模块下的所有 Repository 类注入到容器中。
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddRepositories(IServiceCollection services)
    {
        var assembly = typeof(PersistenceModule).Assembly;
        var types = assembly
            .GetTypes()
            .Where(p =>
                !p.IsGenericType &&
                !p.IsAbstract &&
                p.IsClass &&
                p.Name.EndsWith("Repository"))
            .ToList();

        foreach (var type in types)
        {
            var baseType = type.GetInterfaces().FirstOrDefault(p => p.Name == $"I{type.Name}");
            if (baseType != null)
            {
                services.AddScoped(baseType, type);
            }
        }
        return services;
    }
}
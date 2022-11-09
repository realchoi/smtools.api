using SmTools.Api.Application;
using SmTools.Api.Filters;
using SpringMountain.Modularity;
using SpringMountain.Modularity.Attribute;

namespace SmTools.Api;

/// <summary>
/// Startup 模块
/// </summary>
[DependsOn(typeof(ApplicationModule))]
public class StartupModule : CoreModuleBase
{
    private readonly IConfiguration _configuration;

    public StartupModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 配置服务容器
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceCollectionContext context)
    {
        var services = context.Services;

        #region Route
        services.AddCors(options =>
        {
            options.AddPolicy("default", policy =>
            {
                policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });

        services.AddMvc(options =>
        {
            options.Filters.Add<UowFilter>();
        });
        #endregion

        #region Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Version = "v1",
                Title = "SmTools.Api"
            });
        });
        #endregion
    }

    /// <summary>
    /// 配置中间件
    /// </summary>
    /// <param name="context"></param>
    public override void Configure(ApplicationBuilderContext context)
    {
        var app = context.ApplicationBuilder;
        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmTools.Api v1");
            });
        }

        app.UseCors("default");

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

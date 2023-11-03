using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmTools.Api.Application;
using SmTools.Api.Core.Helpers;
using SmTools.Api.Extensions;
using SmTools.Api.Filters;
using SmTools.Api.Routings;
using SpringMountain.Framework.Snowflake;
using SpringMountain.Modularity;
using SpringMountain.Modularity.Attribute;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using SmTools.Api.Middlewares;

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
        var configuration = context.Configuration;

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
            // 路由统一添加前缀
            options.Conventions.Insert(0, new RouteConvention(new RouteAttribute("api")));
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
            Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
            {
                options.IncludeXmlComments(file, true);
            });
            options.OperationFilter<AddResponseHeadersFilter>();
            options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");
            options.SchemaFilter<SwaggerAddEnumDescriptionFilter>();
            // 权限 token
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "请输入带有 Bearer 的 Token，形如“Bearer {Token}”",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
        });

        #endregion

        #region JWT Token

        services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // 是否验证 Issuer
                    ValidIssuer = configuration["Jwt:Issuer"], // 发行人 Issuer
                    ValidateAudience = true, // 是否验证 Audience
                    ValidAudience = configuration["Jwt:Audience"], // 订阅人 Audience
                    ValidateIssuerSigningKey = true, // 是否验证 SecurityKey
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), // SecurityKey
                    ValidateLifetime = true, // 是否验证失效时间
                    ClockSkew = TimeSpan.Zero, // 过期时间容错值，解决服务器端时间不同步问题（秒）
                    RequireExpirationTime = true
                };
            });

        // 注入 JwtHelper，单例模式
        services.AddSingleton(new JwtHelper(configuration));
        services.AddSingleton(new HashingHelper());

        #endregion

        #region 雪花算法

        services.AddSnowflake(option => { option.WorkId = 1; });

        #endregion

        #region HttpContextAccessor

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        #endregion

        #region MediatR 中介者

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies()
            .Where(c => c.FullName != null && c.FullName.StartsWith("SmTools.Api")).ToArray());

        #endregion

        #region 模型绑定异常处理配置

        services.ConfigureModelBindingErrorHandling();

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

        app.Use(next => context =>
        {
            /*
             * 说明：在记录审计日志时，需要在 Action 执行完成后再次读取 context.Request.Body（详见 AuditActionAttribute 自定义特性过滤器）。
             * 一般情况下，context.Request.Body 流对象不允许被重复读取，
             * 这是因为 context.Request.Body 流对象的 Position 和 Seek 都是不允许进行修改操作的，一旦操作会直接抛出异常。
             * 若希望重复读取 context.Request.Body 流对象，微软引入了 context.Request 的扩展方法 EnableBuffering()，
             * 调用这个方法后，我们可以通过重置流对象的读取位置，来实现 context.Request.Body 的重复读取。
             * 注意：EnableBuffering() 方法每次请求设置一次即可，即在准备读取 context.Request.Body 之前设置。
             */
            context.Request.EnableBuffering();
            return next(context);
        });

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            // 使用 swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmTools.Api v1"); });
        }
        else
        {
            app.UseHsts();
        }

        // 跨域
        app.UseCors("default");

        app.UseRouting();

        // 先认证
        app.UseAuthentication();
        // 再授权
        app.UseAuthorization();

        // 防止重复请求
        app.UsePreventRepeatSubmitMiddleware();

        // 自定义异常处理中间件
        app.UseErrorHandlingMiddleware();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
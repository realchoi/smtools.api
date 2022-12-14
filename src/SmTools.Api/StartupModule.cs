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
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "请输入带有 Bearer 的 Token，形如“Bearer {Token}”",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
        });
        #endregion

        #region JWT Token
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,  // 是否验证 Issuer
                    ValidIssuer = configuration["Jwt:Issuer"],  // 发行人 Issuer
                    ValidateAudience = true,    // 是否验证 Audience
                    ValidAudience = configuration["Jwt:Audience"],  // 订阅人 Audience
                    ValidateIssuerSigningKey = true,    // 是否验证 SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),    // SecurityKey
                    ValidateLifetime = true,    // 是否验证失效时间
                    ClockSkew = TimeSpan.Zero,   // 过期时间容错值，解决服务器端时间不同步问题（秒）
                    RequireExpirationTime = true
                };
            });

        // 注入 JwtHelper，单例模式
        services.AddSingleton(new JwtHelper(configuration));
        services.AddSingleton(new HashingHelper());
        #endregion

        #region 雪花算法
        services.AddSnowflake(option =>
        {
            option.WorkId = 1;
        });
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

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            // 使用 swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmTools.Api v1");
            });
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

        // 自定义异常处理中间件
        app.UseErrorHandlingMiddleware();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

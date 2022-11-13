using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmTools.Api.Application;
using SmTools.Api.Filters;
using SmTools.Api.Helpers;
using SmTools.Api.Routings;
using SpringMountain.Modularity;
using SpringMountain.Modularity.Attribute;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
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
                    ClockSkew = TimeSpan.FromSeconds(300),   // 过期时间容错值，解决服务器端时间不同步问题（秒）
                    RequireExpirationTime = true
                };
            });

        // 注入 JwtHelper，单例模式
        services.AddSingleton(new JwtHelper(configuration));
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

        // 先认证
        app.UseAuthentication();
        // 再授权
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

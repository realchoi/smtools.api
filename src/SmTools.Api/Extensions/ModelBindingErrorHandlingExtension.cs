using Microsoft.AspNetCore.Mvc;
using SpringMountain.Api.Exceptions.Contracts;
using SpringMountain.Api.Exceptions.Contracts.Dtos;

namespace SmTools.Api.Extensions;

/// <summary>
/// 模型绑定异常处理配置扩展
/// </summary>
public static class ModelBindingErrorHandlingExtension
{
    /// <summary>
    /// 配置模型绑定异常处理
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureModelBindingErrorHandling(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(option =>
        {
            option.InvalidModelStateResponseFactory = actionContext =>
            {
                var error = actionContext.ModelState?.FirstOrDefault(e => e.Value?.Errors.Count > 0).Value;
                // 记录日志
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<StartupModule>>();
                logger.LogError(new EventId(0), error?.Errors.First().Exception, $"入参解析失败：{error?.Errors.First().ErrorMessage}");
                logger.LogDebug($"入参解析失败：{error?.Errors.First().ErrorMessage}");
                // 返回自定义异常信息
                var errorOutput = new ErrorOutput(
                    InternalErrorCode.InternalServerError,
                    "服务器内部异常",
                    $"入参解析失败：{error?.Errors.First().ErrorMessage}")
                {
                    Details = error?.Errors.First().Exception?.ToString()
                };
                return new BadRequestObjectResult(errorOutput);
            };
        });
    }
}

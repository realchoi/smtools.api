using SmTools.Api.Middlewares;

namespace SmTools.Api.Extensions;

/// <summary>
/// 中间件扩展
/// </summary>
public static class MiddlewareExtension
{
    /// <summary>
    /// 使用自定义异常处理中间件
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandlingMiddleware>();
    }

    /// <summary>
    /// 防止重复请求
    /// </summary>
    /// <param name="app"></param>
    public static void UsePreventRepeatSubmitMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<PreventRepeatSubmitMiddleware>();
    }
}
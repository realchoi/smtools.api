using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SpringMountain.Api.Exceptions.Contracts;
using SpringMountain.Api.Exceptions.Contracts.Dtos;
using SpringMountain.Api.Exceptions.Contracts.Exceptions;

namespace SmTools.Api.Middlewares;

/// <summary>
/// 异常处理中间件
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="next">调用下一步中间件的委托</param>
    /// <param name="logger">日志对象</param>
    /// <param name="env">程序环境</param>
    public ErrorHandlingMiddleware(RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// 执行当前中间件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// 异常处理
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // 记录日志
        _logger.LogError(new EventId(0), exception, exception.Message);
        _logger.LogDebug(exception.StackTrace);
        context.Response.ContentType = "application/json";

        var serializerSetting = new JsonSerializerSettings
        {
            // 首字母小写
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        if (exception is ApiBaseException apiException)
        {
            context.Response.StatusCode = (int)apiException.HttpCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(
                new ErrorOutput(apiException.ErrorCode, apiException.Status, apiException.Message)
                {
                    Details = apiException?.Details
                }, serializerSetting));
        }
        else
        {
            context.Response.StatusCode = 500;
            var message = _env.IsDevelopment() ? exception.GetBaseException().Message : "服务器内部发生错误，请联系管理员";
            var details = _env.IsDevelopment() ? new List<string> { exception.StackTrace } : null;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(
                new ErrorOutput(InternalErrorCode.InternalServerError, "INTERNAL_SERVER_ERROR", message)
                {
                    Details = details
                }, serializerSetting));
        }
    }
}

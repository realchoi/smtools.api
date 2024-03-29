﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SpringMountain.Api.Exceptions.Contracts;
using SpringMountain.Api.Exceptions.Contracts.Dtos;
using SpringMountain.Api.Exceptions.Contracts.Exceptions;

namespace SmTools.Api.Filters;

/// <summary>
/// 异常处理过滤器
/// </summary>
public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ErrorHandlingFilterAttribute> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="logger">日志对象</param>
    /// <param name="webHostEnvironment">程序使用环境</param>
    public ErrorHandlingFilterAttribute(ILogger<ErrorHandlingFilterAttribute> logger,
        IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// 异常发生时
    /// </summary>
    /// <param name="context"></param>
    public override void OnException(ExceptionContext context)
    {
        // 记录日志
        _logger.LogError(new EventId(0), context.Exception, context.Exception.ToString());

        // 将请求上下文中的异常是否已处理置为 true
        context.ExceptionHandled = true;

        // 自定义异常基类
        if (context.Exception is ApiBaseException exception)
        {
            context.HttpContext.Response.StatusCode = (int)exception.HttpCode;
            context.Result = new JsonResult(new ErrorOutput(exception.ErrorCode, exception.Status, exception.Message)
            {
                Details = exception?.Details
            });
        }
        else
        {
            context.HttpContext.Response.StatusCode = 500;
            context.Result = new JsonResult(new ErrorOutput(
                InternalErrorCode.InternalServerError,
                "INTERNAL_SERVER_ERROR",
                context.Exception.GetBaseException().Message)
            {
                Details = _webHostEnvironment.IsDevelopment()
                    ? new List<string>() { context.Exception.StackTrace }
                    : null
            });
        }
    }
}

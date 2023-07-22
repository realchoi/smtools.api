using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SmTools.Api.Filters;
using SpringMountain.Api.Exceptions.Contracts;
using SpringMountain.Api.Exceptions.Contracts.Dtos;

namespace SmTools.Api.Middlewares;

/// <summary>
/// 防止重复提交中间件
/// </summary>
public class PreventRepeatSubmitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCache _distributedCache;

    public PreventRepeatSubmitMiddleware(RequestDelegate next,
        IDistributedCache distributedCache)
    {
        _next = next;
        _distributedCache = distributedCache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            await _next(context);
            return;
        }

        // 获取 RepeatSubmitAttribute（防止重复提交属性）
        var preventRepeatSubmitAttribute = endpoint.Metadata.OfType<PreventRepeatSubmitAttribute>().FirstOrDefault();

        // 如果不需要处理重复提交，则正常处理业务逻辑
        if (preventRepeatSubmitAttribute == null)
        {
            await _next(context);
            return;
        }

        var isRepeatSubmit = await IsRepeatSubmit(context, preventRepeatSubmitAttribute);
        if (!isRepeatSubmit)
        {
            // using var response = new MemoryStream();
            // var oldResponseBody = context.Response.Body;
            // context.Response.Body = response;
            // 调用下一个中间件
            await _next(context);
            // // 获取 action 返回值
            // using var reader = new StreamReader(response);
            // response.Seek(0, SeekOrigin.Begin);
            // var responseJson = await reader.ReadToEndAsync();
            // // 返回
            // await response.CopyToAsync(oldResponseBody);
            // context.Response.ContentType = "application/json";
            // context.Response.StatusCode = (int)HttpStatusCode.OK;
            // context.Response.Body = oldResponseBody;
            // await context.Response.WriteAsync(responseJson);
        }
        else
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                new ErrorOutput(InternalErrorCode.DuplicateRequest, "DUPLICATE_REQUEST",
                    preventRepeatSubmitAttribute.Message)
                {
                    Details = new List<string>(0)
                }, settings));
        }
    }

    private const string CacheRepeatKey = "RepeatSubmitDataCache";

    /// <summary>
    /// 判定是否是重复提交
    /// </summary>
    /// <param name="context"></param>
    /// <param name="preventRepeatSubmitAttribute"></param>
    /// <returns></returns>
    private async Task<bool> IsRepeatSubmit(HttpContext context,
        PreventRepeatSubmitAttribute preventRepeatSubmitAttribute)
    {
        // 本次请求参数（包括请求的路径）（hash 值）
        var currentParameterHash = await GetCurrentParameterHash(context);
        // 本次请求系统时间
        var currentUnixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // 缓存的键
        var currentCacheKey = $"{CacheRepeatKey}_{currentParameterHash}";
        // 查询当前请求是否被缓存过
        var cache = await _distributedCache.GetStringAsync(currentCacheKey);
        if (!cache.IsNullOrEmpty())
        {
            var previousRequest = JsonConvert.DeserializeObject<RepeatSubmitDto>(cache);
            // 如果能获取到相同的 hash 值，表明当前请求的路径和参数在之前已请求过
            if (previousRequest != null)
            {
                // 此时再判断是否大于设置的允许间隔时间
                var interval = currentUnixTimestamp - previousRequest.Timestamp;
                // 两次相同参数的请求，如果间隔时间较小，则判定为重复请求
                if (interval < preventRepeatSubmitAttribute.Interval)
                {
                    return true;
                }
            }
        }

        var currentRequest = new RepeatSubmitDto
        {
            ParameterHash = currentParameterHash,
            Timestamp = currentUnixTimestamp
        };
        await _distributedCache.RemoveAsync(currentCacheKey);
        await _distributedCache.SetStringAsync(currentCacheKey, JsonConvert.SerializeObject(currentRequest),
            new DistributedCacheEntryOptions
            {
                // 缓存过期时间：在设置的重复请求间隔时间的基础上 + 100 毫秒
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(preventRepeatSubmitAttribute.Interval + 100)
            });

        return false;
    }


    /// <summary>
    /// 获取当前请求参数的 hash 值
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static async Task<string> GetCurrentParameterHash(HttpContext context)
    {
        var url = context.Request.Path;
        var queryData = JsonConvert.SerializeObject(context.Request.Query);
        var formData = string.Empty;
        var bodyData = string.Empty;

        if (context.Request.HasFormContentType)
        {
            // 读取 form
            formData = JsonConvert.SerializeObject(context.Request.Form);
        }
        else if (context.Request.HasJsonContentType())
        {
            // 读取 body
            var stream = context.Request.Body;
            var buffer = new byte[1024 * 4];
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            // 将读取的数据复制到另一个流中
            using var memoryStream = new MemoryStream(buffer, 0, bytesRead);
            memoryStream.Seek(0, SeekOrigin.Begin);

            // 解析请求体中的数据
            using var streamReader = new StreamReader(memoryStream, Encoding.UTF8);
            bodyData = await streamReader.ReadToEndAsync();

            // 将流的位置重置为初始位置
            stream.Seek(0, SeekOrigin.Begin);
        }

        byte[] sha1Hash = SHA256.HashData(Encoding.UTF8.GetBytes(url + queryData + formData + bodyData));
        var hash = Convert.ToBase64String(sha1Hash);
        return hash;
    }
}

/// <summary>
/// 重复请求 DTO
/// </summary>
internal class RepeatSubmitDto
{
    /// <summary>
    /// 请求参数（包括请求路径）（hash 值）
    /// </summary>
    public string ParameterHash { get; set; }

    /// <summary>
    /// 请求时间戳
    /// </summary>
    public long Timestamp { get; set; }
}
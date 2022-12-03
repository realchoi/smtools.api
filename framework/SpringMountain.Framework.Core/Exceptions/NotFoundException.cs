using SpringMountain.Framework.Exceptions.Enums;
using System.Collections;
using System.Net;

namespace SpringMountain.Framework.Exceptions;

/// <summary>
/// 未找到异常
/// </summary>
public class NotFoundException : ApiBaseException
{
    public override HttpStatusCode HttpCode { get; set; } = HttpStatusCode.NotFound;


    public override InternalErrorCode ErrorCode { get; set; } = InternalErrorCode.NotFound;


    public override string Status { get; } = "NOT_FOUND";


    public override string Message { get; } = "未找到";


    public NotFoundException()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
        Message = message;
    }

    public NotFoundException(string message, Exception exception)
        : base(message, exception)
    {
        Message = message;
    }

    public NotFoundException(string message, IEnumerable details)
        : base(message)
    {
        Message = message;
        base.Details = details;
    }

    public NotFoundException(string message, IEnumerable details, Exception exception)
        : base(message, exception)
    {
        Message = message;
        base.Details = details;
    }
}

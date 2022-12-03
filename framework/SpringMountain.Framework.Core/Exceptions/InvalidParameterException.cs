using SpringMountain.Framework.Exceptions;
using SpringMountain.Framework.Exceptions.Enums;
using System.Collections;
using System.Net;

namespace SpringMountain.Framework.Core.Exceptions;

/// <summary>
/// 无效参数异常
/// </summary>
public class InvalidParameterException : ApiBaseException
{
    public override HttpStatusCode HttpCode { get; set; } = HttpStatusCode.BadRequest;

    public override InternalErrorCode ErrorCode { get; set; } = InternalErrorCode.InvalidParameter;

    public override string Status { get; } = "INVALID_PARAMETER";

    public override string Message { get; } = "无效参数";

    public InvalidParameterException()
    {
    }

    public InvalidParameterException(string message)
        : base(message)
    {
        Message = message;
    }

    public InvalidParameterException(string message, Exception exception)
        : base(message, exception)
    {
        Message = message;
    }

    public InvalidParameterException(string message, IEnumerable details)
        : base(message)
    {
        Message = message;
        base.Details = details;
    }

    public InvalidParameterException(string message, IEnumerable details, Exception exception)
        : base(message, exception)
    {
        Message = message;
        base.Details = details;
    }
}

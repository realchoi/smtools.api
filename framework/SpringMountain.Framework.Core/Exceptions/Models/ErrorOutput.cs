using SpringMountain.Framework.Exceptions.Enums;
using System.Collections;

namespace SpringMountain.Framework.Exceptions.Models;

public class ErrorOutput
{
    /// <summary>
    /// 错误码
    /// </summary>
    public InternalErrorCode Code { get; set; }

    /// <summary>
    /// 状态描述
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 错误详情
    /// </summary>
    public IEnumerable? Details { get; set; }
}

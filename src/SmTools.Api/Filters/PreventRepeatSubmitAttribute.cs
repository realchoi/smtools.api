namespace SmTools.Api.Filters;

/// <summary>
/// 防止重复提交
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class PreventRepeatSubmitAttribute : Attribute
{
    private const int IntervalConst = 3000;
    private const string MessageConst = "操作过快，请稍后再试";

    /// <summary>
    /// 防止重复提交
    /// </summary>
    /// <param name="interval">间隔时间（ms），小于此时间视为重复提交（默认 3000 毫秒）</param>
    /// <param name="message">提示信息</param>
    public PreventRepeatSubmitAttribute(int interval = IntervalConst, string message = MessageConst)
    {
        this.Interval = interval;
        this.Message = message;
    }

    /// <summary>
    /// 间隔时间（ms），小于此时间视为重复提交（默认 3000 毫秒）
    /// </summary>
    public int Interval { get; }

    /// <summary>
    /// 提示信息
    /// </summary>
    public string Message { get; }
}
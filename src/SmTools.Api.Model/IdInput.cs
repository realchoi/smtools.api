namespace SmTools.Api.Model;

/// <summary>
/// 带有主键的接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IIdInput<T>
{
    T Id { get; set; }
}

/// <summary>
/// 带有主键的基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class IdInput<T> : IIdInput<T>
{
    public T Id { get; set; }
}

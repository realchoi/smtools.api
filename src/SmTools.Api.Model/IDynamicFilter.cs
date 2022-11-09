namespace SmTools.Api.Model;

/// <summary>
/// 动态过滤条件接口
/// </summary>
public interface IDynamicFilter
{
    /// <summary>
    /// 动态过滤条件
    /// </summary>
    public List<DynamicFilterItem> DynamicFilters { get; set; }
}

/// <summary>
/// 动态过滤条件条目
/// </summary>
public class DynamicFilterItem
{
    /// <summary>
    /// 字段名
    /// </summary>
    public string PropName { get; set; }

    /// <summary>
    /// 操作符
    /// </summary>
    public string Operator { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// 操作符
    /// </summary>
    public class OperatorConst
    {
        /// <summary>
        /// 包含
        /// </summary>
        public const string Like = "like";

        /// <summary>
        /// 相等
        /// </summary>
        public const string Equal = "==";

        /// <summary>
        /// 不相等
        /// </summary>
        public const string NotEq = "!=";

        /// <summary>
        /// 小于
        /// </summary>
        public const string Less = "<";

        /// <summary>
        /// 大于
        /// </summary>
        public const string Greater = ">";

        /// <summary>
        /// 小于等于
        /// </summary>
        public const string LessOrEqual = "<=";

        /// <summary>
        /// 大于等于
        /// </summary>
        public const string GreaterOrEqual = ">=";

        /// <summary>
        /// 包含
        /// </summary>
        public const string In = "in";

        /// <summary>
        /// 区间 >= and <=
        /// </summary>
        public const string Between = "between";
    }
}

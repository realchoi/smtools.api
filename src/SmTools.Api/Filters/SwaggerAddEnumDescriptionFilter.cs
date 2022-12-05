using Microsoft.OpenApi.Models;
using SmTools.Api.Model;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;

namespace SmTools.Api.Filters;

/// <summary>
/// swagger枚举文档注释
/// </summary>
public class SwaggerAddEnumDescriptionFilter : ISchemaFilter
{
    /// <summary>
    /// 处理枚举的swagger注释
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var all = AllEnumSummary;

            var des = Enum.GetNames(context.Type).Select(c => new
            {
                name = c,
                summary = AllEnumSummary.FirstOrDefault(x => x.name.Contains($"{context.Type.FullName?.Replace("+", ".")}.{c}")).summary ?? c
            })
                .Select(c => $"{c.summary}={Convert.ToInt64(Enum.Parse(context.Type, c.name))}")
                .JoinAsString(",");
            schema.Description += $"({des})";
        }
    }

    /// <summary>
    /// 所有的枚举注释
    /// </summary>
    public static readonly List<(string name, string summary)> AllEnumSummary = GetAllEnumSummary();

    /// <summary>
    /// 获取枚举的NameValue
    /// </summary>
    /// <param name="enumType"></param>
    /// <returns></returns>
    public static List<NameValueDto<int>> GetEnumNameValues(Type enumType)
    {
        var nameValues = Enum.GetNames(enumType).Select(c => new
        {
            Description = enumType.GetField(c)?.GetCustomAttribute<DescriptionAttribute>()?.Description,
            Name = AllEnumSummary.FirstOrDefault(x => x.Item1.Contains($"{enumType.FullName?.Replace("+", ".")}.{c}")).Item2 ?? c,
            Value = Convert.ToInt32(Enum.Parse(enumType, c))
        });
        return nameValues.Select(c => new NameValueDto<int>() { Name = c.Description ?? c.Name, Value = c.Value })
            .ToList();
    }

    /// <summary>
    /// 获取枚举的NameValue
    /// </summary>
    /// <param name="constType"></param>
    /// <returns></returns>
    public static List<NameValueDto<string>> GetConstNameValues(Type constType)
    {
        var nameValues = constType.GetFields().Select(c => new
        {
            Description = c.GetCustomAttribute<DescriptionAttribute>()?.Description,
            Name = AllEnumSummary.FirstOrDefault(x => x.name.Contains($"{constType.FullName?.Replace("+", ".")}.{c.Name}")).summary ?? c.Name,
            Value = (string)c.GetValue(null)
        });
        return nameValues.Select(c => new NameValueDto<string>() { Name = c.Description ?? c.Name, Value = c.Value })
            .ToList();
    }

    private static List<(string name, string summary)> GetAllEnumSummary()
    {
        var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml");
        foreach (var file in files)
        {
            var xml = XElement.Load(file);
        }

        var list = files.Select(XElement.Load).SelectMany(c => c.Element("members")?.Elements())
            .Select(c => new
            {
                name = c.Attribute("name")?.Value,
                summary = c.Element("summary")?.Value?.Trim('\n').Trim()
            }).Where(c => c.name.StartsWith("F:") || c.name.StartsWith("T:") || c.name.Contains("Const"))
            .Select(c => (c.name, c.summary)).ToList();
        return list;
    }
}

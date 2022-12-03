using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SmTools.Api.Persistence.Tools;

public class EfValueTool
{
    public static ValueConverter GetJsonValueConverterByType(Type type)
    {
        if (type == typeof(string))
        {
            return null;
        }
        return typeof(EfValueTool).GetMethod(nameof(GetJsonValueConverter))!.MakeGenericMethod(type).Invoke(null, null) as ValueConverter;
    }

    public static ValueComparer GetListToJsonValueComparerByType(Type type)
    {
        if (type.IsAssignableTo(typeof(IEnumerable<>)) && type != typeof(string))
        {
            return typeof(EfValueTool).GetMethod(nameof(GetListToJsonValueComparer))!
                .MakeGenericMethod(type.GetGenericArguments()[0]).Invoke(null, null) as ValueComparer;
        }
        return typeof(EfValueTool).GetMethod(nameof(GetJsonValueComparer))!.MakeGenericMethod(type).Invoke(null, null) as
            ValueComparer;
    }

    public static ValueConverter<T, string> GetJsonValueConverter<T>() where T : new()
    {
        var setting = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() }
        };
        return new ValueConverter<T, string>(
            v => JsonConvert.SerializeObject(v, setting),
            v => JsonConvert.DeserializeObject<T>(v, setting));
    }

    public static ValueComparer<T> GetJsonValueComparer<T>()
    {
        return new ValueComparer<T>(
            (c1, c2) => c1.Equals(c2)
                        && JsonConvert.SerializeObject(c1) == JsonConvert.SerializeObject(c2),
            c => c.GetHashCode());
    }

    public static ValueComparer<IEnumerable<T>> GetListToJsonValueComparer<T>()
    {
        return new ValueComparer<IEnumerable<T>>(
            (c1, c2) => c1.SequenceEqual(c2)
                        && JsonConvert.SerializeObject(c1) == JsonConvert.SerializeObject(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());
    }
}

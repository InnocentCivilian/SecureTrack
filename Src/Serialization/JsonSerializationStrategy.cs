using System.Reflection;
using System.Text.Json;

namespace SecureTrack.Serialization;

/// <summary>
/// JSON-based serialization strategy.
/// </summary>
public class JsonSerializationStrategy : ISerializationStrategy
{
    private string SerializeObjectWithSortedKeys(object record)
    {
        var sortedDict = record.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .OrderBy(p => p.Name)
            .ToDictionary(
                prop => prop.Name,
                prop => prop.GetValue(record)
            );

        return JsonSerializer.Serialize(sortedDict);
    }
    public string Serialize(object record)
    {
        return SerializeObjectWithSortedKeys(record);
    }
}
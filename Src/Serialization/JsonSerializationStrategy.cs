namespace SecureTrack.Serialization;

/// <summary>
/// JSON-based serialization strategy.
/// </summary>
public class JsonSerializationStrategy<T> : ISerializationStrategy<T>
{
    public string Serialize(T record)
    {
        return System.Text.Json.JsonSerializer.Serialize(record);
    }
}
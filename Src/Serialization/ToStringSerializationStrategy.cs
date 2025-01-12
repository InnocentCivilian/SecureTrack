namespace SecureTrack.Serialization;

/// <summary>
/// ToString-based serialization strategy.
/// </summary>
public class ToStringSerializationStrategy<T> : ISerializationStrategy<T>
{
    public string Serialize(T record)
    {
        return record?.ToString() ?? string.Empty;
    }
}

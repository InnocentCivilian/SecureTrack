namespace SecureTrack.Serialization;

/// <summary>
/// ToString-based serialization strategy.
/// </summary>
public class ToStringSerializationStrategy : ISerializationStrategy
{
    public string Serialize(object record)
    {
        return record?.ToString() ?? string.Empty;
    }
}

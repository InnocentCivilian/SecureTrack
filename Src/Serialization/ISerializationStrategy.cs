namespace SecureTrack.Serialization;

/// <summary>
/// Interface for serialization strategies.
/// </summary>
public interface ISerializationStrategy<T>
{
    string Serialize(T record);
}
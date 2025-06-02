namespace SecureTrack.Serialization;

/// <summary>
/// Interface for serialization strategies.
/// </summary>
public interface ISerializationStrategy
{
    string Serialize(object record);
}
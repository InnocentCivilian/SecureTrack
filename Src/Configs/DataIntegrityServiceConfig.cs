using SecureTrack.Enums;
namespace SecureTrack.Configs
{
    /// <summary>
    /// Configuration for the DataIntegrityService.
    /// </summary>
    public class DataIntegrityServiceConfig
    {
        public Type HashAlgorithmType { get; set; }
        public Type SerializationStrategyType { get; set; }
        public void Validate()
        {
            if (HashAlgorithmType == null)
                throw new InvalidOperationException("Hashing algorithm must be configured.");

            if (SerializationStrategyType == null)
                throw new InvalidOperationException("Serialization strategy must be configured.");
        }
    }
}
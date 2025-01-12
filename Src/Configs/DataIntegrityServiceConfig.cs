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
    }
}
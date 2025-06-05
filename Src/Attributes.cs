namespace SecureTrack;

public class Attributes
{
    /// <summary>
    /// Marks an entity or property to skip data integrity validation and hash calculation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class SkipIntegrityCheckAttribute : Attribute
    {
    }
}
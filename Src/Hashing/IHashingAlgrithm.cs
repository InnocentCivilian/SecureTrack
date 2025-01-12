namespace SecureTrack.Hashing;

/// <summary>
/// Interface for hashing algorithms.
/// </summary>
public interface IHashingAlgorithm
{
    string ComputeHash(string input);
}
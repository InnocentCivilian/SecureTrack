using System.Text;

namespace SecureTrack.Hashing;
/// <summary>
/// Implementation of SHA256 hashing algorithm.
/// </summary>
public class SHA256HashingAlgorithm : IHashingAlgorithm
{
    public string ComputeHash(string input)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hashBytes);
    }
}
using System.Text;

namespace SecureTrack.Hashing;


/// <summary>
/// Implementation of SHA512 hashing algorithm.
/// </summary>
public class SHA512HashingAlgorithm : IHashingAlgorithm
{
    public string ComputeHash(string input)
    {
        using var sha512 = System.Security.Cryptography.SHA512.Create();
        byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hashBytes);
    }
}
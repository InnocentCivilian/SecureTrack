using SecureTrack.Exceptions;

namespace SecureTrack.Services.Interfaces;

/// <summary>
/// Provides generic data integrity services for verifying and maintaining the integrity of data records.
/// </summary>
/// <typeparam name="T">The type of data record the integrity services apply to.</typeparam>
public interface IDataIntegrityService<T>
{
    /// <summary>
    /// Generates a digest for a given record using the specified secret salt.
    /// </summary>
    /// <param name="recordData">The data record for which the digest is to be generated.</param>
    /// <param name="secretSalt">The secret salt used to enhance the security of the digest.</param>
    /// <returns>The generated digest as a string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    string GenerateDigest(T recordData, string secretSalt);

    /// <summary>
    /// Validates the stored digest against the generated digest from the record data and secret salt.
    /// </summary>
    /// <param name="storedDigest">The digest that was stored previously and needs to be validated.</param>
    /// <param name="recordData">The current data record for validation.</param>
    /// <param name="secretSalt">The secret salt used to generate the original digest.</param>
    /// <returns>true if the digests match indicating integrity; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
    bool ValidateDigest(string storedDigest, T recordData, string secretSalt);

    /// <summary>
    /// Handles violations of data integrity by providing a method to respond to such events.
    /// </summary>
    /// <param name="integrityException">The exception that represents the integrity violation.</param>
    void HandleIntegrityViolation(IntegrityViolationException integrityException);
}
/// <summary>
/// Provides data integrity services for verifying and maintaining the integrity of data records.
/// </summary>
public interface IDataIntegrityService
{
    /// <summary>
    /// Generates a digest (hash) for a given record using the specified secret salt.
    /// </summary>
    /// <param name="recordData">The data record to generate the digest for.</param>
    /// <param name="secretSalt">The secret salt to enhance the security of the digest.</param>
    /// <returns>The generated digest as a string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when recordData or secretSalt is null or empty.</exception>
    string GenerateDigest(object recordData, string secretSalt);

    /// <summary>
    /// Validates the stored digest against the generated digest from the record data and secret salt.
    /// </summary>
    /// <param name="storedDigest">The previously stored digest to validate against.</param>
    /// <param name="recordData">The current data record for validation.</param>
    /// <param name="secretSalt">The secret salt used when generating the original digest.</param>
    /// <returns>True if the digests match (indicating data integrity); otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when storedDigest, recordData, or secretSalt is null or empty.</exception>
    bool ValidateDigest(string storedDigest, object recordData, string secretSalt);

    /// <summary>
    /// Handles violations of data integrity, typically by logging or triggering custom logic.
    /// </summary>
    /// <param name="integrityException">The exception representing the integrity violation.</param>
    /// <exception cref="ArgumentNullException">Thrown when integrityException is null.</exception>
    void HandleIntegrityViolation(IntegrityViolationException integrityException);
}
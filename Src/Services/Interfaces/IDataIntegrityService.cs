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
/// Defines the contract for computing and validating data integrity using digest-based verification.
/// </summary>
public interface IDataIntegrityService
{
    /// <summary>
    /// Serializes the given data record to a string representation using the configured serialization strategy.
    /// </summary>
    /// <param name="recordData">The object to serialize.</param>
    /// <returns>A string representation of the object.</returns>
    string Serialize(object recordData);

    /// <summary>
    /// Generates a cryptographic digest from a record's data and an optional secret salt.
    /// </summary>
    /// <param name="recordData">The data object to hash.</param>
    /// <param name="secretSalt">Optional secret salt to add to the input for hash strengthening.</param>
    /// <returns>A digest string representing the object's integrity fingerprint.</returns>
    string GenerateDigest(object recordData, string secretSalt = null);

    /// <summary>
    /// Validates the stored digest of a record against its computed value. Throws an exception if they do not match.
    /// </summary>
    /// <param name="recordData">The record object implementing <see cref="IHashable"/> to validate.</param>
    /// <param name="secretSalt">Optional secret salt used during digest generation.</param>
    /// <exception cref="IntegrityViolationException">Thrown if the integrity check fails.</exception>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CheckIntegrityAsync(object recordData, string secretSalt = null);

    /// <summary>
    /// Updates the digest of the record to reflect its current state by recalculating the hash and setting it on the object.
    /// </summary>
    /// <param name="recordData">The record implementing <see cref="IHashable"/> to update.</param>
    /// <param name="secretSalt">Optional secret salt used during digest generation.</param>
    void UpdateDigest(object recordData, string secretSalt = null);
}
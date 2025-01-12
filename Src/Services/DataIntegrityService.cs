using SecureTrack.Exceptions;
using SecureTrack.Services.Interfaces;
using SecureTrack.Hashing;
using SecureTrack.Serialization;

namespace SecureTrack.Services
{

   /// <summary>
    /// Implementation of IDataIntegrityService for managing data integrity.
    /// </summary>
    /// <typeparam name="T">The type of data record the integrity services apply to.</typeparam>
    public class DataIntegrityService<T> : IDataIntegrityService<T>
    {
        private readonly IHashingAlgorithm _hashingAlgorithm;
        private readonly ISerializationStrategy<T> _serializationStrategy;

        public DataIntegrityService(IHashingAlgorithm hashingAlgorithm, ISerializationStrategy<T> serializationStrategy)
        {
            _hashingAlgorithm = hashingAlgorithm ?? throw new ArgumentNullException(nameof(hashingAlgorithm));
            _serializationStrategy = serializationStrategy ?? throw new ArgumentNullException(nameof(serializationStrategy));
        }

        /// <summary>
        /// Generates a digest for a given record using the specified secret salt.
        /// </summary>
        /// <param name="recordData">The data record for which the digest is to be generated.</param>
        /// <param name="secretSalt">The secret salt used to enhance the security of the digest.</param>
        /// <returns>The generated digest as a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        public string GenerateDigest(T recordData, string secretSalt)
        {
            if (recordData == null) throw new ArgumentNullException(nameof(recordData));
            if (string.IsNullOrEmpty(secretSalt)) throw new ArgumentNullException(nameof(secretSalt));

            string recordString = _serializationStrategy.Serialize(recordData);
            string combinedData = recordString + secretSalt;
            return _hashingAlgorithm.ComputeHash(combinedData);
        }

        /// <summary>
        /// Validates the stored digest against the generated digest from the record data and secret salt.
        /// </summary>
        /// <param name="storedDigest">The digest that was stored previously and needs to be validated.</param>
        /// <param name="recordData">The current data record for validation.</param>
        /// <param name="secretSalt">The secret salt used to generate the original digest.</param>
        /// <returns>true if the digests match indicating integrity; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        public bool ValidateDigest(string storedDigest, T recordData, string secretSalt)
        {
            if (string.IsNullOrEmpty(storedDigest)) throw new ArgumentNullException(nameof(storedDigest));
            if (recordData == null) throw new ArgumentNullException(nameof(recordData));
            if (string.IsNullOrEmpty(secretSalt)) throw new ArgumentNullException(nameof(secretSalt));

            string generatedDigest = GenerateDigest(recordData, secretSalt);
            return string.Equals(storedDigest, generatedDigest, StringComparison.Ordinal);
        }

        /// <summary>
        /// Handles violations of data integrity by providing a method to respond to such events.
        /// </summary>
        /// <param name="integrityException">The exception that represents the integrity violation.</param>
        public void HandleIntegrityViolation(IntegrityViolationException integrityException)
        {
            if (integrityException == null) throw new ArgumentNullException(nameof(integrityException));

            // Log the violation (could be replaced with a logging framework)
            Console.WriteLine($"Integrity violation detected: {integrityException.Message}");

            // Additional handling logic can be added here, e.g., alerting, auditing, etc.
        }
    }
}

using System.Reflection;
using SecureTrack.Exceptions;
using SecureTrack.Services.Interfaces;
using SecureTrack.Hashing;
using SecureTrack.Serialization;
using Microsoft.Extensions.Logging;

namespace SecureTrack.Services
{
    /// <summary>
    /// Implementation of IDataIntegrityService for managing data integrity.
    /// </summary>
    /// <typeparam name="T">The type of data record the integrity services apply to.</typeparam>
    public class DataIntegrityService<T> : IDataIntegrityService<T>
    {
        private readonly IHashingAlgorithm _hashingAlgorithm;
        private readonly ISerializationStrategy _serializationStrategy;
        private readonly ILogger<DataIntegrityService<T>> _logger;

        public DataIntegrityService(IHashingAlgorithm hashingAlgorithm, ISerializationStrategy serializationStrategy,
            ILogger<DataIntegrityService<T>> logger)
        {
            _hashingAlgorithm = hashingAlgorithm ?? throw new ArgumentNullException(nameof(hashingAlgorithm));
            _serializationStrategy =
                serializationStrategy ?? throw new ArgumentNullException(nameof(serializationStrategy));
            _logger = logger;
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
            _logger.LogError("Integrity violation detected: {message}", integrityException.Message);

            // Additional handling logic can be added here, e.g., alerting, auditing, etc.
        }
    }

    /// <summary>
    /// Provides an implementation of the IDataIntegrityService interface for managing data integrity across any type of record.
    /// </summary>
    /// <summary>
    /// Provides an implementation of <see cref="IDataIntegrityService"/> for generating and validating digests of data records.
    /// </summary>
    public class DataIntegrityService : IDataIntegrityService
    {
        private readonly IHashingAlgorithm _hashingAlgorithm;
        private readonly ISerializationStrategy _serializer;
        private readonly ILogger<DataIntegrityService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataIntegrityService"/> class.
        /// </summary>
        /// <param name="hashingAlgorithm">The hashing algorithm used for digest computation.</param>
        /// <param name="serializer">The serialization strategy used to convert objects into strings.</param>
        /// <param name="logger">The logger for recording integrity validation failures or events.</param>
        public DataIntegrityService(
            IHashingAlgorithm hashingAlgorithm,
            ISerializationStrategy serializer,
            ILogger<DataIntegrityService> logger)
        {
            _hashingAlgorithm = hashingAlgorithm ?? throw new ArgumentNullException(nameof(hashingAlgorithm));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public string Serialize(object recordData)
        {
            return _serializer.Serialize(recordData);
        }

        /// <inheritdoc/>
        public string GenerateDigest(object recordData, string secretSalt = null)
        {
            if (recordData == null) throw new ArgumentNullException(nameof(recordData));

            var serialized = _serializer.Serialize(recordData);
            var input = string.IsNullOrEmpty(secretSalt) ? serialized : serialized + secretSalt;
            return _hashingAlgorithm.ComputeHash(input);
        }

        /// <inheritdoc/>
        public Task CheckIntegrityAsync(object recordData, string secretSalt = null)
        {
            if (recordData == null)
                throw new ArgumentNullException(nameof(recordData));

            var type = recordData.GetType();

            // Skip check if [SkipIntegrityCheck] is applied
            if (type.GetCustomAttribute<Attributes.SkipIntegrityCheckAttribute>() != null)
            {
                _logger.LogDebug("Skipping integrity check for {EntityType} due to [SkipIntegrityCheck]",
                    type.Name);
                return Task.CompletedTask;
            }

            if (recordData is not IHashable hashable)
                throw new InvalidOperationException("Record must implement IHashable to support integrity checks.");

            var expected = hashable.Hash;
            var actual = GenerateDigest(recordData, secretSalt);

            if (!string.Equals(expected, actual, StringComparison.Ordinal))
            {
                _logger.LogError("Data integrity violation detected on entity of type {EntityType}.", type.Name);
                throw new IntegrityViolationException($"Hash mismatch detected for entity of type {type.Name}.");
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void UpdateDigest(object recordData, string secretSalt = null)
        {
            var type = recordData.GetType();

            if (type.GetCustomAttribute<Attributes.SkipIntegrityCheckAttribute>() != null)
            {
                _logger.LogDebug("Skipping digest update for {EntityType} due to [SkipIntegrityCheck]", type.Name);
                return;
            }

            if (recordData is not IHashable hashable)
                throw new InvalidOperationException("Record must implement IHashable to support digest updates.");

            hashable.Hash = GenerateDigest(recordData, secretSalt);
        }
    }
}
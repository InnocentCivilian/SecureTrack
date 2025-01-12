using Microsoft.Extensions.DependencyInjection;
using SecureTrack.Configs;
using SecureTrack.Hashing;
using SecureTrack.Serialization;
using SecureTrack.Services;
using SecureTrack.Services.Interfaces;

namespace SecureTrack.Extensions;

/// <summary>
/// Implementation of IDataIntegrityService for managing data integrity.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataIntegrityService(
        this IServiceCollection services,
        DataIntegrityServiceConfig config,
        Action<IServiceCollection> configure = null)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));

        // Register hashing algorithm
        if (config.HashAlgorithmType == null || !typeof(IHashingAlgorithm).IsAssignableFrom(config.HashAlgorithmType))
        {
            throw new ArgumentException("Invalid or missing HashingAlgorithmType in configuration.");
        }
        services.AddSingleton(typeof(IHashingAlgorithm), config.HashAlgorithmType);

        // Register serialization strategy
        if (config.SerializationStrategyType == null || !config.SerializationStrategyType.IsGenericTypeDefinition)
        {
            throw new ArgumentException("SerializationStrategyType must be a generic type definition.");
        }
        services.AddSingleton(typeof(ISerializationStrategy<>), config.SerializationStrategyType);

        // Allow custom registrations
        configure?.Invoke(services);

        // Register the non-generic DataIntegrityService
        services.AddSingleton(typeof(IDataIntegrityService<>), typeof(DataIntegrityService<>));

        return services;
    }
}
//sample usage :
// services.AddDataIntegrityService(
//     new DataIntegrityServiceConfig
//     {
//         HashingAlgorithmType = typeof(CustomHashingAlgorithm),
//         SerializationStrategyType = typeof(CustomSerializationStrategy<>),
//     });

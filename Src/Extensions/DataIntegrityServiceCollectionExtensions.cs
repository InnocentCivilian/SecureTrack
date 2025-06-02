using Microsoft.Extensions.DependencyInjection;
using SecureTrack.Services;
using SecureTrack.Services.Interfaces;

namespace SecureTrack.Extensions;

/// <summary>
/// Implementation of IDataIntegrityService for managing data integrity.
/// </summary>
/// <summary>
/// Provides extension methods for registering the DataIntegrityService into the DI container.
/// </summary>
public static class DataIntegrityServiceCollectionExtensions
{
    /// <summary>
    /// Registers the generic IDataIntegrityService; for the specified type T and returns the builder for further configuration.
    /// </summary>
    /// <typeparam name="T">The type of records the integrity service will manage.</typeparam>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>A DataIntegrityServiceBuilder for configuring the service.</returns>
    public static DataIntegrityServiceBuilder AddDataIntegrityService<T>(this IServiceCollection services)
    {
        services.AddSingleton<IDataIntegrityService<T>, DataIntegrityService<T>>();
        return new DataIntegrityServiceBuilder(services);
    }

    /// <summary>
    /// Registers the non-generic IDataIntegrityService for general-purpose use with object types and returns the builder for further configuration.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>A DataIntegrityServiceBuilder for configuring the service.</returns>
    public static DataIntegrityServiceBuilder AddDataIntegrityService(this IServiceCollection services)
    {
        services.AddSingleton<IDataIntegrityService, DataIntegrityService>();
        return new DataIntegrityServiceBuilder(services);
    }
}
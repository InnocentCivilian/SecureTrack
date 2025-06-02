using Microsoft.Extensions.DependencyInjection;
using SecureTrack.Configs;
using SecureTrack.Hashing;
using SecureTrack.Serialization;

namespace SecureTrack.Extensions;

public class DataIntegrityServiceBuilder
{
    private readonly IServiceCollection _services;

    public DataIntegrityServiceBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public DataIntegrityServiceBuilder UseSha256()
    {
        _services.AddSingleton<IHashingAlgorithm, SHA256HashingAlgorithm>();
        return this;
    }

    public DataIntegrityServiceBuilder UseSha512()
    {
        _services.AddSingleton<IHashingAlgorithm, SHA512HashingAlgorithm>();
        return this;
    }

    public DataIntegrityServiceBuilder UseJsonSerialization()
    {
        _services.AddSingleton<ISerializationStrategy, JsonSerializationStrategy>();
        return this;
    }
    public DataIntegrityServiceBuilder UseRawStringSerialization()
    {
        _services.AddSingleton<ISerializationStrategy, ToStringSerializationStrategy>();
        return this;
    }

    public DataIntegrityServiceBuilder UseCustomHashing<TCustom>() where TCustom : class, IHashingAlgorithm
    {
        _services.AddSingleton<IHashingAlgorithm, TCustom>();
        return this;
    }

    public DataIntegrityServiceBuilder UseCustomSerialization<T, TCustom>()
        where TCustom : class, ISerializationStrategy
    {
        _services.AddSingleton<ISerializationStrategy, TCustom>();
        return this;
    }

    public DataIntegrityServiceBuilder UseDefaultSettings<T>()
    {
        return UseSha256().UseJsonSerialization();
    }
}

//sample usage :
//  Register type-specific integrity service
// services.AddDataIntegrityService<MyEntity>()
//     .UseSha512()
//     .UseJsonSerialization();
//
//  Register general-purpose (non-generic) integrity service
// services.AddDataIntegrityService()
//     .UseSha256()
//     .UseRawStringSerialization();

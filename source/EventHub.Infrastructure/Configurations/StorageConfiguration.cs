using EventHub.Shared.Settings;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace EventHub.Infrastructure.Configurations;

public static class StorageConfiguration
{
    public static IServiceCollection ConfigureMinioStorage(this IServiceCollection services)
    {
        MinioStorage minioStorage = services.GetOptions<MinioStorage>("MinioStorage");

        services.AddMinio(configureClient => configureClient
            .WithEndpoint(minioStorage.Endpoint)
            .WithCredentials(minioStorage.AccessKey, minioStorage.SecretKey)
            .WithSSL(minioStorage.Secure)
            .Build());

        return services;
    }
}

using BuildingBlocks.Core.Web.Extenions;
using Microsoft.AspNetCore.Builder;
using Minio;
using Minio.AspNetCore.HealthChecks;
using Minio.Exceptions;
using Polly;

namespace BlobStorage;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddBlobStorage(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(serviceProvider =>
          {
              var configuration = builder.Configuration.BindOptions<MinioOptions>();
              var minioClient = new MinioClient()
                  .WithEndpoint(configuration.Uri ?? "")
                  .WithCredentials(configuration.Username, configuration.Password)
                  .Build();
              minioClient.WithTimeout(5000);
              minioClient.WithRetryPolicy(async callback => await Policy
                                              .Handle<ConnectionException>()
                                              .WaitAndRetryAsync(3, retryCount => TimeSpan.FromSeconds(retryCount * 2))
                                              .ExecuteAsync(async () => await callback()));
              return minioClient;
          });
        builder.Services.AddTransient<IMinioService, MinioService>();
        return builder;
    }

    public static IServiceCollection AddBlobStorageHealthCheck(this IServiceCollection services)
    {
        services.AddHealthChecks().AddMinio(
            serviceProvider => serviceProvider.GetRequiredService<MinioClient>(),
            tags: new[] { "live" });
        return services;
    }
}

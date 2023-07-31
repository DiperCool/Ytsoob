using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.Postgres;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Ytsoobers.Shared.Contracts;
using Ytsoob.Services.Ytsoobers.Shared.Data;

namespace Ytsoob.Services.Ytsoobers.Shared.Extensions.WebApplicationBuilderExtensions;

public static partial class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddStorage(this WebApplicationBuilder builder)
    {
        AddPostgresWriteStorage(builder.Services, builder.Configuration);
        AddMongoReadStorage(builder.Services, builder.Configuration);

        return builder;
    }

    private static void AddPostgresWriteStorage(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("PostgresOptions:UseInMemory"))
        {
            services.AddDbContext<YtsoobersesDbContext>(
                options => options.UseInMemoryDatabase("ECommerce.Services.Ytsoob.Services.Ytsoobers")
            );

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<YtsoobersesDbContext>()!);
        }
        else
        {
            services.AddPostgresDbContext<YtsoobersesDbContext>();
        }

        services.AddScoped<IYtsoobersDbContext>(provider => provider.GetRequiredService<YtsoobersesDbContext>());
    }

    private static void AddMongoReadStorage(IServiceCollection services, IConfiguration configuration)
    {

    }
}

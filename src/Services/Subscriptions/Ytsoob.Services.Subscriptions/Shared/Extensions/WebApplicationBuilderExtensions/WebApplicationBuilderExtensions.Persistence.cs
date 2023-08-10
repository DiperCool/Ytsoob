using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.Postgres;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Subscriptions.Shared.Contracts;
using Ytsoob.Services.Subscriptions.Shared.Data;

namespace Ytsoob.Services.Subscriptions.Shared.Extensions.WebApplicationBuilderExtensions;

public static partial class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddStorage(this WebApplicationBuilder builder)
    {
        AddPostgresWriteStorage(builder.Services, builder.Configuration);

        return builder;
    }

    private static void AddPostgresWriteStorage(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("PostgresOptions:UseInMemory"))
        {
            services.AddDbContext<SubscriptionsDbContext>(
                options => options.UseInMemoryDatabase("ECommerce.Services.Ytsoob.Services.Subscriptions")
            );

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<SubscriptionsDbContext>()!);
        }
        else
        {
            services.AddPostgresDbContext<SubscriptionsDbContext>();
        }

        services.AddScoped<ISubscriptionsDbContext>(provider => provider.GetRequiredService<SubscriptionsDbContext>());
    }
}

using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.Postgres;
using BuildingBlocks.Persistence.Mongo;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Payment.Shared.Contracts;
using Ytsoob.Services.Payment.Shared.Data;

namespace Ytsoob.Services.Payment.Shared.Extensions.WebApplicationBuilderExtensions;

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
            services.AddDbContext<PaymentDbContext>(options => options.UseInMemoryDatabase("Ytsoob.Services.Payment"));

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<PaymentDbContext>()!);
        }
        else
        {
            services.AddPostgresDbContext<PaymentDbContext>();
        }

        services.AddScoped<IPaymentDbContext>(provider => provider.GetRequiredService<PaymentDbContext>());
    }
}

using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core;
using Stripe;
using Ytsoob.Services.Payment.Shared.Contracts;
using Ytsoob.Services.Payment.Shared.Extensions.WebApplicationBuilderExtensions;
using Ytsoob.Services.Payment.Shared.Extensions.WebApplicationExtensions;
using Ytsoob.Services.Payment.Shared.Services;

namespace Ytsoob.Services.Payment.Shared;

public class SharedModulesConfiguration : ISharedModulesConfiguration
{
    public const string CustomerModulePrefixUri = "api/v{version:apiVersion}/customers";

    public WebApplicationBuilder AddSharedModuleServices(WebApplicationBuilder builder)
    {
        StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("StripeSettings:ApiKey");
        builder.AddInfrastructure();
        builder.AddStorage();

        builder.Services.AddTransient<IPaymentService, PaymentService>();
        return builder;
    }

    public async Task<WebApplication> ConfigureSharedModule(WebApplication app)
    {
        await app.UseInfrastructure();

        ServiceActivator.Configure(app.Services);

        await app.ApplyDatabaseMigrations();

        return app;
    }

    public IEndpointRouteBuilder MapSharedModuleEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "/",
                (HttpContext context) =>
                {
                    var requestId = context.Request.Headers.TryGetValue(
                        "X-Request-InternalCommandId",
                        out var requestIdHeader
                    )
                        ? requestIdHeader.FirstOrDefault()
                        : string.Empty;

                    return $"Customers Service Apis, RequestId: {requestId}";
                }
            )
            .ExcludeFromDescription();

        return endpoints;
    }
}

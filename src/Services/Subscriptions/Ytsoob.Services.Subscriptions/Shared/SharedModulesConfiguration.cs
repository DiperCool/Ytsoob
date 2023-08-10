using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core;
using Ytsoob.Services.Subscriptions.Shared.Extensions.WebApplicationBuilderExtensions;
using Ytsoob.Services.Subscriptions.Shared.Extensions.WebApplicationExtensions;

namespace Ytsoob.Services.Subscriptions.Shared;

public class SharedModulesConfiguration : ISharedModulesConfiguration
{
    public const string ModulePrefixUri = "api/v{version:apiVersion}";

    public WebApplicationBuilder AddSharedModuleServices(WebApplicationBuilder builder)
    {
        builder.AddInfrastructure();

        builder.AddStorage();

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

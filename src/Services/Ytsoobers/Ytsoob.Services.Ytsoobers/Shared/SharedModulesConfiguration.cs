using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core;
using Ytsoob.Services.Ytsoobers.Shared.Extensions.WebApplicationBuilderExtensions;
using Ytsoob.Services.Ytsoobers.Shared.Extensions.WebApplicationExtensions;

namespace Ytsoob.Services.Ytsoobers.Shared;

public class SharedModulesConfiguration : ISharedModulesConfiguration
{
    public const string YtsooberModulePrefixUri = "api/v{version:apiVersion}/ytsoobers";

    public WebApplicationBuilder AddSharedModuleServices(WebApplicationBuilder builder)
    {
        builder.AddInfrastructure();
        builder.Services.AddServices();
        builder.AddStorage();

        return builder;
    }

    public async Task<WebApplication> ConfigureSharedModule(WebApplication app)
    {
        await app.UseInfrastructure();

        ServiceActivator.Configure(app.Services);

        await app.ApplyDatabaseMigrations();
        await app.SeedData();

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

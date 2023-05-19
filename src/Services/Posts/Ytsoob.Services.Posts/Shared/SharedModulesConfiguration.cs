using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core;
using Ytsoob.Services.Posts.Shared.Extensions.WebApplicationBuilderExtensions;
using Ytsoob.Services.Posts.Shared.Extensions.WebApplicationExtensions;

namespace Ytsoob.Services.Posts.Shared;

public class SharedModulesConfiguration : ISharedModulesConfiguration
{
    public const string PostModulePrefixUri = "api/v{version:apiVersion}/posts";

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

using BuildingBlocks.Abstractions.Web.Module;
using Ytsoob.Services.Posts.Shared;

namespace Ytsoob.Services.Posts.Reactions;

public class ReactionConfings : IModuleConfiguration
{
    public const string Tag = "Reactions";
    public const string ReactionsPrefixUri = $"{SharedModulesConfiguration.PostModulePrefixUri}/reactions";

    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        // builder.Services.AddScoped<IDataSeeder, ProductDataSeeder>();
        // builder.Services.AddSingleton<IEventMapper, ProductEventMapper>();
        return builder;
    }

    public Task<WebApplication> ConfigureModule(WebApplication app)
    {
        return Task.FromResult(app);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // create a new sub group for each version

        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#route-groups
        // https://github.com/dotnet/aspnet-api-versioning/blob/main/examples/AspNetCore/WebApi/MinimalOpenApiExample/Program.cs

        return endpoints;
    }
}

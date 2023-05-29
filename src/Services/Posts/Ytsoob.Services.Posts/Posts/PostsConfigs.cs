using BuildingBlocks.Abstractions.Web.Module;
using Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1;
using Ytsoob.Services.Posts.Shared;

namespace Ytsoob.Services.Posts.Posts;

public class PostsConfigs : IModuleConfiguration
{
    public const string Tag = "Post";
    public const string PostPrefixUri = $"{SharedModulesConfiguration.PostModulePrefixUri}/posts";

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
        var postVersionGroup = endpoints.MapApiGroup(Tag).WithTags(Tag);

        // create a new sub group for each version
        var productsGroupV1 = postVersionGroup.MapGroup(PostPrefixUri).HasApiVersion(1.0);

        // create a new sub group for each version

        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#route-groups
        // https://github.com/dotnet/aspnet-api-versioning/blob/main/examples/AspNetCore/WebApi/MinimalOpenApiExample/Program.cs
        productsGroupV1.MapCreatePostEndpoint();
        productsGroupV1.MapUpdatePostEndpoint();

        return endpoints;
    }
}

using BuildingBlocks.Abstractions.Web.Module;
using Ytsoob.Services.Ytsoobers.Shared;

namespace Ytsoob.Services.Ytsoobers.Profiles;

public class ProfilesConfig : IModuleConfiguration
{
    public const string Tag = "Profiles";
    public const string ProfilesPrefixUri = $"{SharedModulesConfiguration.YtsooberModulePrefixUri}/profiles";
    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        return builder;
    }

    public Task<WebApplication> ConfigureModule(WebApplication app)
    {
        return Task.FromResult(app);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}

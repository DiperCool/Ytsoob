using Ytsoob.Services.Ytsoobers.Shared.Contracts;
using Ytsoob.Services.Ytsoobers.Shared.Services;

namespace Ytsoob.Services.Ytsoobers.Shared.Extensions.WebApplicationBuilderExtensions;

public static partial class WebApplicationBuilderExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAvatarStorage, AvatarStorage>();
    }
}

using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.Postgres;
using BuildingBlocks.Persistence.Mongo;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Shared.Contracts;
using Ytsoob.Services.Posts.Shared.Data;

namespace Ytsoob.Services.Posts.Shared.Extensions.WebApplicationBuilderExtensions;

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
            services.AddDbContext<PostsDbContext>(
                options => options.UseInMemoryDatabase("Ytsoob.Services.Ytsoob.Services.Posts")
            );

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<PostsDbContext>()!);
        }
        else
        {
            services.AddPostgresDbContext<PostsDbContext>();
        }

        services.AddScoped<IPostsDbContext>(provider => provider.GetRequiredService<PostsDbContext>());
    }

}

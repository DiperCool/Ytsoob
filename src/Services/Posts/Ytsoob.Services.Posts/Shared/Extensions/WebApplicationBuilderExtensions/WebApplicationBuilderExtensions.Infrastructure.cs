using BlobStorage;
using BuildingBlocks.Caching;
using BuildingBlocks.Caching.Behaviours;
using BuildingBlocks.Core.IdsGenerator;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Core.Registrations;
using BuildingBlocks.Core.Web.Extenions;
using BuildingBlocks.Email;
using BuildingBlocks.HealthCheck;
using BuildingBlocks.Integration.MassTransit;
using BuildingBlocks.Logging;
using BuildingBlocks.Messaging.Persistence.Postgres.Extensions;
using BuildingBlocks.OpenTelemetry;
using BuildingBlocks.Persistence.EfCore.Postgres;
using BuildingBlocks.Security.Extensions;
using BuildingBlocks.Security.Jwt;
using BuildingBlocks.Swagger;
using BuildingBlocks.Validation;
using BuildingBlocks.Web.Extensions;
using Ytsoob.Services.Posts.Polls.Algs;
using Ytsoob.Services.Posts.Shared.Contracts;
using Ytsoob.Services.Posts.Shared.Services;
using Ytsoob.Services.Posts.Users;

namespace Ytsoob.Services.Posts.Shared.Extensions.WebApplicationBuilderExtensions
{
    public static partial class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
        {
            SnowFlakIdGenerator.Configure(1);
            builder.Services.AddCore(builder.Configuration);

            builder.Services.AddCustomJwtAuthentication(builder.Configuration);
            builder.Services.AddCustomAuthorization(
                rolePolicies: new List<RolePolicy>
                {
                    new(CustomersConstants.Role.Admin, new List<string> { CustomersConstants.Role.Admin }),
                    new(CustomersConstants.Role.User, new List<string> { CustomersConstants.Role.User })
                }
            );
            builder.Services.AddTransient<IReactionService, ReactionService>();
            builder.Configuration.AddEnvironmentVariables("Ytsoob_customers_env_");

            // https://github.com/tonerdo/dotnet-env
            DotNetEnv.Env.TraversePath().Load();

            if (builder.Environment.IsTest() == false)
            {
                builder.AddCustomHealthCheck(healthChecksBuilder =>
                {
                    var postgresOptions = builder.Configuration.BindOptions<PostgresOptions>();
                    var rabbitMqOptions = builder.Configuration.BindOptions<RabbitMqOptions>();

                    healthChecksBuilder
                        .AddNpgSql(
                            postgresOptions.ConnectionString,
                            name: "CustomersService-Postgres-Check",
                            tags: new[] { "postgres", "database", "infra", "customers-service", "live", "ready" }
                        )
                        .AddRabbitMQ(
                            rabbitMqOptions.ConnectionString,
                            name: "CustomersService-RabbitMQ-Check",
                            timeout: TimeSpan.FromSeconds(3),
                            tags: new[] { "rabbitmq", "bus", "infra", "customers-service", "live", "ready" }
                        );
                });
            }

            builder.Services.AddEmailService(builder.Configuration);

            builder.AddCompression();
            builder.AddCustomProblemDetails();

            builder.AddCustomOpenTelemetry();

            builder.AddCustomSerilog();

            builder.AddCustomVersioning();
            builder.AddCustomSwagger(typeof(CustomersAssemblyInfo).Assembly);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IContentBlobStorage, ContentBlobStorage>();
            builder.Services.AddCqrs(
                pipelines: new[]
                {
                    typeof(RequestValidationBehavior<,>),
                    typeof(StreamRequestValidationBehavior<,>),
                    typeof(StreamLoggingBehavior<,>),
                    typeof(StreamCachingBehavior<,>),
                    typeof(LoggingBehavior<,>),
                    typeof(CachingBehavior<,>),
                    typeof(InvalidateCachingBehavior<,>),
                    typeof(EfTxBehavior<,>)
                }
            );

            builder.Services.AddPostgresMessagePersistence(builder.Configuration);

            // https://blog.maartenballiauw.be/post/2022/09/26/aspnet-core-rate-limiting-middleware.html
            builder.AddCustomRateLimit();
            builder.AddBlobStorage();
            builder.AddCustomMassTransit(
                (context, cfg) =>
                {
                    cfg.AddUsersEndpoints(context);
                },
                autoConfigEndpoints: false
            );

            builder.Services.AddCustomValidators(Assembly.GetExecutingAssembly());

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddTransient<ICacheYtsooberOptions, CacheYtsooberOptions>();
            builder.AddCustomCaching();

            builder.Services.AddTransient<IPollStrategy, SingleAnswerPollAlg>();
            builder.Services.AddTransient<IPollStrategy, MultiplePollAnswerAlg>();
            return builder;
        }
    }
}

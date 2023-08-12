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
using BuildingBlocks.Persistence.Mongo;
using BuildingBlocks.Security.Extensions;
using BuildingBlocks.Security.Jwt;
using BuildingBlocks.Swagger;
using BuildingBlocks.Validation;
using BuildingBlocks.Web.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Ytsoob.Services.Payment.Shared.Extensions.WebApplicationBuilderExtensions;

public static partial class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        SnowFlakIdGenerator.Configure(2);

        builder.Services.AddCore(builder.Configuration);

        builder.Services.AddCustomJwtAuthentication(builder.Configuration);
        builder.Services.AddCustomAuthorization(
            rolePolicies: new List<RolePolicy>
            {
                new(PaymentConstants.Role.Admin, new List<string> { PaymentConstants.Role.Admin }),
                new(PaymentConstants.Role.User, new List<string> { PaymentConstants.Role.User })
            }
        );

        // https://www.michaco.net/blog/EnvironmentVariablesAndConfigurationInASPNETCoreApps#environment-variables-and-configuration
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0#non-prefixed-environment-variables
        builder.Configuration.AddEnvironmentVariables("ecommerce_customers_env_");

        // https://github.com/tonerdo/dotnet-env
        DotNetEnv.Env.TraversePath().Load();

        if (builder.Environment.IsTest() == false)
        {
            builder.AddCustomHealthCheck(healthChecksBuilder =>
            {
                var postgresOptions = builder.Configuration.BindOptions<PostgresOptions>();
                var rabbitMqOptions = builder.Configuration.BindOptions<RabbitMqOptions>();
                var mongoOptions = builder.Configuration.BindOptions<MongoOptions>();

                healthChecksBuilder
                    .AddNpgSql(
                        postgresOptions.ConnectionString,
                        name: "CustomersService-Postgres-Check",
                        tags: new[] { "postgres", "database", "infra", "customers-service" }
                    )
                    .AddRabbitMQ(
                        rabbitMqOptions.ConnectionString,
                        name: "CustomersService-RabbitMQ-Check",
                        timeout: TimeSpan.FromSeconds(3),
                        tags: new[] { "rabbitmq", "bus", "infra", "customers-service" }
                    )
                    .AddMongoDb(
                        mongoOptions.ConnectionString,
                        mongoDatabaseName: mongoOptions.DatabaseName,
                        "CustomersService-Mongo-Check",
                        tags: new[] { "mongodb", "database", "infra", "customers-service" }
                    );
            });
        }

        builder.Services.AddEmailService(builder.Configuration);

        builder.AddCompression();
        builder.AddCustomProblemDetails();

        builder.AddCustomOpenTelemetry();

        builder.AddCustomSerilog();

        builder.AddCustomVersioning();
        builder.AddCustomSwagger(typeof(PaymentRoot).Assembly);
        builder.Services.AddHttpContextAccessor();

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

        builder.AddCustomMassTransit((context, cfg) => { }, autoConfigEndpoints: false);

        builder.Services.AddCustomValidators(Assembly.GetExecutingAssembly());

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        builder.AddCustomCaching();

        return builder;
    }
}

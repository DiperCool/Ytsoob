using Ytsoob.Services.Shared.Identity.Users.Events.v1.Integration;
using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Ytsoob.Services.Shared.Ytsoobers.Ytsoobers.Events.v1.Integration;
using Ytsoob.Services.Ytsoobers.Users.Features.RegisteringUser.v1.Events.Integration.External;

namespace Ytsoob.Services.Ytsoobers.Users;

internal static class MassTransitExtensions
{
    internal static void AddUsersEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint(
            nameof(UserRegisteredV1).Underscore(),
            re =>
            {
                // turns off default fanout settings
                re.ConfigureConsumeTopology = false;

                // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
                re.SetQuorumQueue();

                re.Bind(
                    $"{nameof(UserRegisteredV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(UserRegisteredV1).Underscore();
                        e.ExchangeType = ExchangeType.Direct;
                    }
                );

                // https://github.com/MassTransit/MassTransit/discussions/3117
                // https://masstransit-project.com/usage/configuration.html#receive-endpoints
                re.ConfigureConsumer<UserRegisteredConsumer>(context);

                re.RethrowFaultedMessages();

                cfg.Message<YtsooberCreatedV1>(
                    e => e.SetEntityName($"{nameof(YtsooberCreatedV1).Underscore()}.input_exchange")
                ); // name of the primary exchange
                cfg.Publish<YtsooberCreatedV1>(e => e.ExchangeType = ExchangeType.Direct); // primary exchange type
                cfg.Send<YtsooberCreatedV1>(e =>
                {
                    // route by message type to binding fanout exchange (exchange to exchange binding)
                    e.UseRoutingKeyFormatter(sendContext => sendContext.Message.GetType().Name.Underscore());
                });
            }
        );
    }
}

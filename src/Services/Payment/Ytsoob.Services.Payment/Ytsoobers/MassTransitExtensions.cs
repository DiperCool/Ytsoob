using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Ytsoob.Services.Payment.Ytsoobers.Features.CreatingYtsoober.v1.Events.Integration.External;
using Ytsoob.Services.Shared.Ytsoobers.Ytsoobers.Events.v1.Integration;

namespace Ytsoob.Services.Payment.Ytsoobers;

internal static class MassTransitExtensions
{
    internal static void AddYtsoobersEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint(
            nameof(YtsooberCreatedV1).Underscore(),
            re =>
            {
                // turns off default fanout settings
                re.ConfigureConsumeTopology = false;

                // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
                re.SetQuorumQueue();

                re.Bind(
                    $"{nameof(YtsooberCreatedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(YtsooberCreatedV1).Underscore();
                        e.ExchangeType = ExchangeType.Direct;
                    }
                );

                // https://github.com/MassTransit/MassTransit/discussions/3117
                // https://masstransit-project.com/usage/configuration.html#receive-endpoints
                re.ConfigureConsumer<YtsooberCreatedConsumer>(context);

                re.RethrowFaultedMessages();
            }
        );
    }
}

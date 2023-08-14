using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Ytsoob.Services.Payment.Subscriptions.Features.CreatingSubscription.v1.Events.Integration;
using Ytsoob.Services.Payment.Subscriptions.Features.RemovingSubscription.v1.RemoveSubscription.Events.Integration;
using Ytsoob.Services.Payment.Subscriptions.Features.UpdatingSubscription.v1.UpdateSubscription.Events.Integration;
using Ytsoob.Services.Shared.Subscriptions.Subscriptions.Events.v1.Integration;

namespace Ytsoob.Services.Payment.Subscriptions;

internal static class MassTransitExtensions
{
    internal static void AddSubscriptionsEndpoints(
        this IRabbitMqBusFactoryConfigurator cfg,
        IBusRegistrationContext context
    )
    {
        cfg.ReceiveEndpoint(
            $"{nameof(Payment).Underscore()}.{nameof(SubscriptionCreatedV1).Underscore()}",
            re =>
            {
                // turns off default fanout settings
                re.ConfigureConsumeTopology = false;

                // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
                re.SetQuorumQueue();

                re.Bind(
                    $"{nameof(SubscriptionCreatedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(SubscriptionCreatedV1).Underscore();
                        e.ExchangeType = ExchangeType.Direct;
                    }
                );

                // https://github.com/MassTransit/MassTransit/discussions/3117
                // https://masstransit-project.com/usage/configuration.html#receive-endpoints
                re.ConfigureConsumer<SubscriptionCreatedConsumer>(context);

                re.RethrowFaultedMessages();
            }
        );

        cfg.ReceiveEndpoint(
            $"{nameof(Payment).Underscore()}.{nameof(SubscriptionUpdatedV1).Underscore()}",
            re =>
            {
                // turns off default fanout settings
                re.ConfigureConsumeTopology = false;

                // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
                re.SetQuorumQueue();
                re.UseRetry(configurator => configurator.Intervals(TimeSpan.FromSeconds(10)));
                re.Bind(
                    $"{nameof(SubscriptionUpdatedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(SubscriptionUpdatedV1).Underscore();
                        e.ExchangeType = ExchangeType.Direct;
                    }
                );

                // https://github.com/MassTransit/MassTransit/discussions/3117
                // https://masstransit-project.com/usage/configuration.html#receive-endpoints
                re.ConfigureConsumer<SubscriptionUpdatedConsumer>(context);

                re.RethrowFaultedMessages();
            }
        );
        cfg.ReceiveEndpoint(
            $"{nameof(Payment).Underscore()}.{nameof(SubscriptionRemovedV1).Underscore()}",
            re =>
            {
                // turns off default fanout settings
                re.ConfigureConsumeTopology = false;

                // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
                re.SetQuorumQueue();

                re.Bind(
                    $"{nameof(SubscriptionRemovedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(SubscriptionRemovedV1).Underscore();
                        e.ExchangeType = ExchangeType.Direct;
                    }
                );

                // https://github.com/MassTransit/MassTransit/discussions/3117
                // https://masstransit-project.com/usage/configuration.html#receive-endpoints
                re.ConfigureConsumer<RemovedSubscriptionConsumer>(context);

                re.RethrowFaultedMessages();
            }
        );
    }
}

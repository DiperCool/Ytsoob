using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Ytsoob.Services.Posts.Subscriptions.Features.CreatingSubscription.v1.Events.Integration;
using Ytsoob.Services.Posts.Subscriptions.Features.RemovingSubscription.v1.RemoveSubscription.Events.Integration;
using Ytsoob.Services.Posts.Subscriptions.Features.UpdatingSubscription.v1.UpdateSubscription.Events.Integration;
using Ytsoob.Services.Shared.Subscriptions.Subscriptions.Events.v1.Integration;

namespace Ytsoob.Services.Posts.Subscriptions;

internal static class MassTransitExtensions
{
    internal static void AddSubscriptionsEndpoints(
        this IRabbitMqBusFactoryConfigurator cfg,
        IBusRegistrationContext context
    )
    {
        cfg.ReceiveEndpoint(
            nameof(SubscriptionCreatedV1).Underscore(),
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
            nameof(SubscriptioUpdatedV1).Underscore(),
            re =>
            {
                // turns off default fanout settings
                re.ConfigureConsumeTopology = false;

                // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
                re.SetQuorumQueue();

                re.Bind(
                    $"{nameof(SubscriptioUpdatedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(SubscriptioUpdatedV1).Underscore();
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
            nameof(SubscriptionRemovedV1).Underscore(),
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

using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Ytsoob.Services.Shared.Subscriptions.Subscriptions.Events.v1.Integration;

namespace Ytsoob.Services.Subscriptions.Subscriptions;

internal static class MassTransitExtensions
{
    internal static void AddSubscriptionPublishers(this IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Message<SubscriptionCreatedV1>(
            e => e.SetEntityName($"{nameof(SubscriptionCreatedV1).Underscore()}.input_exchange")
        );
        cfg.Publish<SubscriptionCreatedV1>(e => e.ExchangeType = ExchangeType.Direct); // primary exchange type
        cfg.Send<SubscriptionCreatedV1>(e =>
        {
            e.UseRoutingKeyFormatter(context => context.Message.GetType().Name.Underscore());
        });

        cfg.Message<SubscriptioUpdatedV1>(
            e => e.SetEntityName($"{nameof(SubscriptioUpdatedV1).Underscore()}.input_exchange")
        );
        cfg.Publish<SubscriptioUpdatedV1>(e => e.ExchangeType = RabbitMQ.Client.ExchangeType.Direct); // primary exchange type
        cfg.Send<SubscriptioUpdatedV1>(e =>
        {
            e.UseRoutingKeyFormatter(context => context.Message.GetType().Name.Underscore());
        });

        cfg.Message<SubscriptionRemovedV1>(
            e => e.SetEntityName($"{nameof(SubscriptionRemovedV1).Underscore()}.input_exchange")
        );
        cfg.Publish<SubscriptionRemovedV1>(e => e.ExchangeType = RabbitMQ.Client.ExchangeType.Direct); // primary exchange type
        cfg.Send<SubscriptionRemovedV1>(e =>
        {
            e.UseRoutingKeyFormatter(context => context.Message.GetType().Name.Underscore());
        });
    }
}

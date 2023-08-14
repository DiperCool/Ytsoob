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

        cfg.Message<SubscriptionUpdatedV1>(
            e => e.SetEntityName($"{nameof(SubscriptionUpdatedV1).Underscore()}.input_exchange")
        );
        cfg.Publish<SubscriptionUpdatedV1>(e => e.ExchangeType = RabbitMQ.Client.ExchangeType.Direct); // primary exchange type
        cfg.Send<SubscriptionUpdatedV1>(e =>
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

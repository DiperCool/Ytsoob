using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Ytsoob.Services.Shared.Subscriptions.Subscriptions.Events.v1.Integration;
using Ytsoob.Services.Subscriptions.Subscriptions.Features.CreatingSubscription.v1.CreateSubscription;
using Ytsoob.Services.Subscriptions.Subscriptions.Features.RemovingSubscription.v1.RemoveSubscription;
using Ytsoob.Services.Subscriptions.Subscriptions.Features.UpdatingSubscription.v1.UpdateSubscription;

namespace Ytsoob.Services.Subscriptions.Subscriptions;

public class SubscriptionsEventMapper : IIntegrationEventMapper
{
    public IReadOnlyList<IIntegrationEvent?>? MapToIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents)
    {
        return domainEvents.Select(MapToIntegrationEvent).ToList().AsReadOnly();
    }

    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            SubscriptionCreatedDomainEvent e
                => new SubscriptionCreatedV1(e.Id, e.Title, e.Description, e.Photo, e.Price, e.YtsooberId),
            SubscriptionUpdatedDomainEvent e => new SubscriptionUpdatedV1(e.Id, e.Title, e.Description, e.Photo),
            SubscriptionRemovedDomainEvent e => new SubscriptionRemovedV1(e.Id),
            _ => null
        };
    }
}

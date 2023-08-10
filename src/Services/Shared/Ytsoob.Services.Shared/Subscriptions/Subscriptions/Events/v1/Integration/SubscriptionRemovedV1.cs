using BuildingBlocks.Core.Messaging;

namespace Ytsoob.Services.Shared.Subscriptions.Subscriptions.Events.v1.Integration;

public record SubscriptionRemovedV1(long Id) : IntegrationEvent;

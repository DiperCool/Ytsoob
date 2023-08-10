using BuildingBlocks.Core.Messaging;

namespace Ytsoob.Services.Shared.Subscriptions.Subscriptions.Events.v1.Integration;

public record SubscriptioUpdatedV1(long Id, string Title, string Description, string? Photo, decimal Price)
    : IntegrationEvent;

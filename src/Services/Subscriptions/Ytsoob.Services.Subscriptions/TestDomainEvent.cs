using BuildingBlocks.Core.CQRS.Events.Internal;

namespace Ytsoob.Services.Subscriptions;

public record TestDomainEvent(string Data) : DomainEvent;

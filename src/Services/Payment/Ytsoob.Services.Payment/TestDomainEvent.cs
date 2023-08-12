using BuildingBlocks.Core.CQRS.Events.Internal;

namespace Ytsoob.Services.Payment;

public record TestDomainEvent(string Data) : DomainEvent;

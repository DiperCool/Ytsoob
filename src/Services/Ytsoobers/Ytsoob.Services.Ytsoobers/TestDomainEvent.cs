using BuildingBlocks.Core.CQRS.Events.Internal;

namespace Ytsoob.Services.Ytsoobers;

public record TestDomainEvent(string Data) : DomainEvent;

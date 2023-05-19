using BuildingBlocks.Core.CQRS.Events.Internal;

namespace Ytsoob.Services.Posts;

public record TestDomainEvent(string Data) : DomainEvent;

using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Persistence.EventStore;
using BuildingBlocks.Core.CQRS.Events;

namespace BuildingBlocks.Core.Persistence.EventStore;

public record StreamEvent(IDomainEvent Data, IStreamEventMetadata? Metadata = null) : Event, IStreamEvent;

public record StreamEvent<T>(T Data, IStreamEventMetadata? Metadata = null)
    : StreamEvent(Data, Metadata),
        IStreamEvent<T>
    where T : IDomainEvent
{
    public new T Data => (T)base.Data;
}

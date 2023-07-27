using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Ytsoob.Services.Shared.Ytsoobers.Ytsoobers.Events.v1.Integration;
using Ytsoob.Services.Ytsoobers.Ytsoobers.Events;

namespace Ytsoob.Services.Ytsoobers;

public class YtsooberEventMapper : IIntegrationEventMapper
{
    public IReadOnlyList<IIntegrationEvent?>? MapToIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents)
    {
        return domainEvents.Select(MapToIntegrationEvent).ToList().AsReadOnly();
    }

    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
           YtsooberCreated e => new YtsooberCreatedV1(
               e.Id,
               e.IdentityId,
               e.Username,
               e.Email,
               new YtsooberProfile(e.Profile.FirstName, e.Profile.LastName, e.Profile.Avatar)),
           _ => null
        };
    }
}

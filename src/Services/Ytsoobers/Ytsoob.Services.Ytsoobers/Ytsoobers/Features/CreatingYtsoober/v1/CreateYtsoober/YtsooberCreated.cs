using BuildingBlocks.Core.CQRS.Events.Internal;
using Ytsoob.Services.Ytsoobers.Profiles.Dtos.v1;

namespace Ytsoob.Services.Ytsoobers.Ytsoobers.Events;
public record YtsooberCreated(
    long Id,
    Guid IdentityId,
    string? Username,
    string Email,
    ProfileDto Profile
) : DomainEvent;

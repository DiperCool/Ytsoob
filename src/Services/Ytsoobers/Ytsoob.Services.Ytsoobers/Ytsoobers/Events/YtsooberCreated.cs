using BuildingBlocks.Core.CQRS.Events.Internal;
using Ytsoob.Services.Ytsoobers.Profiles.Dtos.V1;
using Ytsoob.Services.Ytsoobers.Ytsoobers.ValueObjects;

namespace Ytsoob.Services.Ytsoobers.Ytsoobers.Events;
public record YtsooberCreated(
    long Id,
    Guid IdentityId,
    string? Username,
    string Email,
    ProfileDto Profile
) : DomainEvent;

using BuildingBlocks.Core.Messaging;

namespace Ytsoob.Services.Shared.Ytsoobers.Ytsoobers.Events.v1.Integration;

public record YtsooberProfile(string FirstName, string LastName, string Avatar);
public record YtsooberCreatedV1(
    long Id,
    Guid IdentityId,
    string? Username,
    string Email,
    YtsooberProfile Profile
) : IntegrationEvent;

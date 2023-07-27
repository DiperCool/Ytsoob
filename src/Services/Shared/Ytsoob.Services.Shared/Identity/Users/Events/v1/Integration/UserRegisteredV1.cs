using BuildingBlocks.Core.Messaging;

namespace Ytsoob.Services.Shared.Identity.Users.Events.v1.Integration;


public record UserRegisteredV1(
    Guid IdentityId,
    long YtsooberId,
    string Username,
    string Email,
    string PhoneNumber,
    List<string>? Roles
) : IntegrationEvent;

using BuildingBlocks.Core.Messaging;

namespace Ytsoob.Services.Shared.Identity.Users.Events.v1.Integration;

public record UserRegisteredV1(
    Guid IdentityId,
    string Email,
    string PhoneNumber,
    List<string>? Roles
) : IntegrationEvent;

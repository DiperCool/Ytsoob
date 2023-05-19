using Ytsoob.Services.Identity.Shared.Models;

namespace Ytsoob.Services.Identity.Users.Features.UpdatingUserState.v1;

public record UpdateUserStateRequest
{
    public UserState UserState { get; init; }
}

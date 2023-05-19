using BuildingBlocks.Core.Messaging;
using Ytsoob.Services.Identity.Shared.Models;

namespace Ytsoob.Services.Identity.Users.Features.UpdatingUserState.v1.Events.Integration;

public record UserStateUpdated(Guid UserId, UserState OldUserState, UserState NewUserState) : IntegrationEvent;

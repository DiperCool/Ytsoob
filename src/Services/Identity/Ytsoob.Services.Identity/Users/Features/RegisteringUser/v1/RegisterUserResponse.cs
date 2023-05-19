using Ytsoob.Services.Identity.Users.Dtos;
using Ytsoob.Services.Identity.Users.Dtos.v1;

namespace Ytsoob.Services.Identity.Users.Features.RegisteringUser.v1;

internal record RegisterUserResponse(IdentityUserDto? UserIdentity);

using Ytsoob.Services.Identity.Users.Dtos;
using Ytsoob.Services.Identity.Users.Dtos.v1;

namespace Ytsoob.Services.Identity.Users.Features.GettingUerByEmail.v1;

public record GetUserByEmailResponse(IdentityUserDto? UserIdentity);

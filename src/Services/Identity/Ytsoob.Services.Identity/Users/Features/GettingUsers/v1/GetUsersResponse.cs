using BuildingBlocks.Core.CQRS.Queries;
using Ytsoob.Services.Identity.Users.Dtos;
using Ytsoob.Services.Identity.Users.Dtos.v1;

namespace Ytsoob.Services.Identity.Users.Features.GettingUsers.v1;

public record GetUsersResponse(ListResultModel<IdentityUserDto> IdentityUsers);

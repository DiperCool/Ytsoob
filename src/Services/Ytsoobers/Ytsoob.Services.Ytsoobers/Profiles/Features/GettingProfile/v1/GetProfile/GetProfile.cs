using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Ytsoobers.Profiles.Dtos.v1;
using Ytsoob.Services.Ytsoobers.Shared.Contracts;
using Ytsoob.Services.Ytsoobers.Ytsoobers.Models;

namespace Ytsoob.Services.Ytsoobers.Profiles.Features.GettingProfile.v1.GetProfile;

public record GetProfile() : ICommand<ProfileDto>;

public class GetProfileEndpoint : IMinimalEndpoint
{
    public string GroupName => ProfilesConfig.Tag;
    public string PrefixRoute => ProfilesConfig.ProfilesPrefixUri;
    public double Version => 1.0;
    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("/", HandleAsync)
            .RequireAuthorization()
            .Produces<ProfileDto>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("GetProfile")
            .WithDisplayName("Get profile.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        return Results.Ok(await commandProcessor.SendAsync(new GetProfile(), cancellationToken));
    }
}

public class GetProfileHandler : ICommandHandler<GetProfile, ProfileDto>
{
    private ICurrentUserService _currentUserService;
    private IYtsoobersDbContext _ytsoobersDbContext;
    private IMapper _mapper;
    public GetProfileHandler(ICurrentUserService currentUserService, IYtsoobersDbContext ytsoobersDbContext, IMapper mapper)
    {
        _currentUserService = currentUserService;
        _ytsoobersDbContext = ytsoobersDbContext;
        _mapper = mapper;
    }

    public async Task<ProfileDto> Handle(GetProfile request, CancellationToken cancellationToken)
    {
        Ytsoober? ytsoober = await _ytsoobersDbContext.Ytsoobers
                              .Include(x => x.Profile)
                              .FirstOrDefaultAsync(x => x.Id == _currentUserService.YtsooberId, cancellationToken: cancellationToken);
        Guard.Against.Null(ytsoober);
        return _mapper.Map<ProfileDto>(ytsoober.Profile);
    }
}

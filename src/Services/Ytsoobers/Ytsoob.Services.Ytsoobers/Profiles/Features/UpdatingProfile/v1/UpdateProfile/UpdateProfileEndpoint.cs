using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using Hellang.Middleware.ProblemDetails;

namespace Ytsoob.Services.Ytsoobers.Profiles.Features.UpdatingProfile.v1.UpdateProfile;

public class UpdateProfileEndpoint : ICommandMinimalEndpoint<UpdateProfile>
{
    public string GroupName => ProfilesConfig.Tag;
    public string PrefixRoute => ProfilesConfig.ProfilesPrefixUri;
    public double Version => 1.0;
    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("CreatePost")
            .WithDisplayName("Create Post.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] UpdateProfile request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));
        await commandProcessor.SendAsync(request, cancellationToken);

        return Results.Ok();
    }
}


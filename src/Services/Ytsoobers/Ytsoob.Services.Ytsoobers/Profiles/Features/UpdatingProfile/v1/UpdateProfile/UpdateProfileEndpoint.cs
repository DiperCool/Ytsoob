using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using Hellang.Middleware.ProblemDetails;

namespace Ytsoob.Services.Ytsoobers.Profiles.Features.UpdatingProfile.v1.UpdateProfile;

public class UpdateProfileEndpoint : IMinimalEndpoint
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
            .WithName("UpdateProfile")
            .WithDisplayName("Update Profile.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromQuery] string firstName,
        [FromQuery] string lastName,
        [FromForm] IFormFile file,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
       await commandProcessor.SendAsync(new UpdateProfile(firstName, lastName, file), cancellationToken);
       return Results.Ok();
    }
}


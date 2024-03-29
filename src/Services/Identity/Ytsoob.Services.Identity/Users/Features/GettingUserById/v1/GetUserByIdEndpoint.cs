using BuildingBlocks.Abstractions.CQRS.Queries;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;
using Ytsoob.Services.Identity.Users.Features.RegisteringUser.v1;

namespace Ytsoob.Services.Identity.Users.Features.GettingUserById.v1;

public static class GetUserByIdEndpoint
{
    internal static RouteHandlerBuilder MapGetUserByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{userId:guid}", GetUserById)
            .AllowAnonymous()
            .Produces<RegisterUserResponse>(StatusCodes.Status200OK)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName("GetUserById")
            .WithDisplayName("Get User by InternalCommandId.")
            .WithMetadata(
                new SwaggerOperationAttribute("Getting User by InternalCommandId", "Getting User by InternalCommandId")
            );
    }

    private static async Task<IResult> GetUserById(
        Guid userId,
        IQueryProcessor queryProcessor,
        CancellationToken cancellationToken
    )
    {
        var result = await queryProcessor.SendAsync(new GetUserById(userId), cancellationToken);

        return Results.Ok(result);
    }
}

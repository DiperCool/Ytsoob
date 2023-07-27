using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using Hellang.Middleware.ProblemDetails;
using Ytsoob.Services.Posts.Posts.Features.UpdatingTextPost.v1;

namespace Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1;
public class UpdatePostEndpoint : ICommandMinimalEndpoint<UpdatePostContentRequest>
{
    public string GroupName => PostsConfigs.Tag;
    public string PrefixRoute => PostsConfigs.PostPrefixUri;
    public double Version => 1.0;
    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPut("/", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("UpdatePost")
            .WithDisplayName("Update Post.");
    }
    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] UpdatePostContentRequest request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        var command = mapper.Map<UpdatePostContent>(request);
        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(UpdatePostEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("PostId", command.PostId))
        {
            var result = await commandProcessor.SendAsync(command, cancellationToken);

            return Results.Ok(result);
        }
    }
}

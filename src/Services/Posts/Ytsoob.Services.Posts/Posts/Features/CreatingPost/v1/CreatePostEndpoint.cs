using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using Hellang.Middleware.ProblemDetails;

namespace Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1;

public class CreatePostEndpoint : ICommandMinimalEndpoint<CreatePostRequest>
{
    public string GroupName => PostsConfigs.Tag;
    public string PrefixRoute => PostsConfigs.PostPrefixUri;
    public double Version => 1.0;
    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/Create", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("CreatePost")
            .WithDisplayName("Create Post.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] CreatePostRequest request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        var command = mapper.Map<CreatePost>(request);
        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(CreatePostEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("PostId", command.Id))
        {
            var result = await commandProcessor.SendAsync(command, cancellationToken);

            return Results.Ok(result);
        }
    }
}

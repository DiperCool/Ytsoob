using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using Hellang.Middleware.ProblemDetails;

namespace Ytsoob.Services.Posts.Contents.Features.UpdatingPostContent.v1;
public class UpdatePostContentEndpoint : ICommandMinimalEndpoint<UpdatePostContentRequest>
{
    public string GroupName => ContentsConfig.Tag;
    public string PrefixRoute => ContentsConfig.PostPrefixUri;
    public double Version => 1.0;
    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPut("/", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("UpdatePostContent")
            .WithDisplayName("Update Post Content.");
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
        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(UpdatePostContentEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("PostId", command.PostId))
        {
            var result = await commandProcessor.SendAsync(command, cancellationToken);

            return Results.Ok(result);
        }
    }
}

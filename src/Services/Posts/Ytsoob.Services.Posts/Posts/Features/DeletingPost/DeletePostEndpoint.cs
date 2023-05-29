using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using Hellang.Middleware.ProblemDetails;

namespace Ytsoob.Services.Posts.Posts.Features.DeletingPost;

public class DeletePostEnpoint : ICommandMinimalEndpoint<DeletePostCommand>
{
    public string GroupName => PostsConfigs.Tag;
    public string PrefixRoute => PostsConfigs.PostPrefixUri;
    public double Version => 1.0;
    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete("/{id}", HandleAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("DeletePost")
            .WithDisplayName("Delete Post.");
    }
    public async Task<IResult> HandleAsync(
        HttpContext context,
        DeletePostCommand request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(DeletePostEnpoint)))
        using (Serilog.Context.LogContext.PushProperty("PostId", request.PostId))
        {
            await commandProcessor.SendAsync(request, cancellationToken);

            return Results.NoContent();
        }
    }


}

using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Comments.Exceptions;
using Ytsoob.Services.Posts.Comments.Models;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Comments.Features.RemovingComment.v1.RemoveComment;

public record RemoveComment(long CommentId) : ITxCommand;

public class RemoveCommentEndpoint : ICommandMinimalEndpoint<RemoveComment>
{
    public string GroupName => CommentsConfigs.Tag;
    public string PrefixRoute => CommentsConfigs.CommentPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete("/Remove", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("RemoveComment")
            .WithDisplayName("Remove Comment.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] RemoveComment request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(RemoveCommentEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("CommentId", request.CommentId))
        {
            var result = await commandProcessor.SendAsync(request, cancellationToken);

            return Results.NoContent();
        }
    }
}

public class RemoveCommentHandler : ICommandHandler<RemoveComment>
{
    private IPostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;

    public RemoveCommentHandler(IPostsDbContext postsDbContext, ICurrentUserService currentUserService)
    {
        _postsDbContext = postsDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(RemoveComment request, CancellationToken cancellationToken)
    {
        BaseComment? baseComment = await _postsDbContext.BaseComments.FirstOrDefaultAsync(
            x => x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (baseComment == null)
        {
            throw new CommentNotFoundException(request.CommentId);
        }

        _postsDbContext.BaseComments.Remove(baseComment);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

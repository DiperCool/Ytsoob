using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Ytsoob.Services.Posts.Comments.Models;
using Ytsoob.Services.Posts.Comments.ValueObjects;
using Ytsoob.Services.Posts.Reactions.Enums;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Comments.Features.RemovingReaction.v1.RemoveReaction;

public record RemoveReactionComment(long CommentId, ReactionType ReactionType) : ITxUpdateCommand;

public class RemoveReactionEndpoint : ICommandMinimalEndpoint<RemoveReactionComment>
{
    public string GroupName => CommentsConfigs.Tag;
    public string PrefixRoute => CommentsConfigs.CommentPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete("/RemoveReaction", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("RemoveReactionComment")
            .WithDisplayName("Remove Reaction.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] RemoveReactionComment request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(RemoveReactionComment)))
        using (Serilog.Context.LogContext.PushProperty("CommentId", request.CommentId))
        {
            var result = await commandProcessor.SendAsync(request, cancellationToken);

            return Results.NoContent();
        }
    }
}

public class RemoveReactionHandler : ICommandHandler<RemoveReactionComment>
{
    private IReactionService _reactionService;
    private ICurrentUserService _currentUserService;
    private ICacheYtsooberReaction _ytsooberReactionCache;

    public RemoveReactionHandler(
        IReactionService reactionService,
        ICurrentUserService currentUserService,
        ICacheYtsooberReaction ytsooberReactionCache
    )
    {
        _reactionService = reactionService;
        _currentUserService = currentUserService;
        _ytsooberReactionCache = ytsooberReactionCache;
    }

    public async Task<Unit> Handle(RemoveReactionComment request, CancellationToken cancellationToken)
    {
        CommentId commentId = CommentId.Of(request.CommentId);
        await _reactionService.RemoveReactionAsync<BaseComment, CommentId>(
            CommentId.Of(request.CommentId),
            _currentUserService.YtsooberId,
            cancellationToken
        );
        await _ytsooberReactionCache.RemoveCache(
            commentId.ToString(),
            _currentUserService.YtsooberId,
            typeof(BaseComment).ToString()
        );
        return Unit.Value;
    }
}

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

namespace Ytsoob.Services.Posts.Comments.Features.AddReaction.v1.AddingReaction;

public record AddReactionComment(long CommentId, ReactionType ReactionType) : ITxUpdateCommand;

public class AddReactionEndpoint : ICommandMinimalEndpoint<AddReactionComment>
{
    public string GroupName => CommentsConfigs.Tag;
    public string PrefixRoute => CommentsConfigs.CommentPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/AddReaction", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("AddReactionComment")
            .WithDisplayName("Add Reaction.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] AddReactionComment request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(AddReactionEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("CommentId", request.CommentId))
        {
            var result = await commandProcessor.SendAsync(request, cancellationToken);

            return Results.NoContent();
        }
    }
}

public class AddReactionHandler : ICommandHandler<AddReactionComment>
{
    private IReactionService _reactionService;
    private ICurrentUserService _currentUserService;
    private ICacheYtsooberReaction _ytsooberReactionCache;

    public AddReactionHandler(
        IReactionService reactionService,
        ICurrentUserService currentUserService,
        ICacheYtsooberReaction ytsooberReactionCache
    )
    {
        _reactionService = reactionService;
        _currentUserService = currentUserService;
        _ytsooberReactionCache = ytsooberReactionCache;
    }

    public async Task<Unit> Handle(AddReactionComment request, CancellationToken cancellationToken)
    {
        CommentId commentId = CommentId.Of(request.CommentId);
        await _reactionService.AddReactionAsync<BaseComment, CommentId>(
            commentId,
            _currentUserService.YtsooberId,
            request.ReactionType,
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

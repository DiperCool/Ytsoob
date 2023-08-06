using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Posts.Features.RemovingReaction.v1.RemoveReaction;

public record RemoveReaction(long Id) : ITxUpdateCommand;

public class RemoveReactionEndpoint : ICommandMinimalEndpoint<RemoveReaction>
{
    public async Task<IResult> HandleAsync(
        HttpContext context,
        RemoveReaction request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        await commandProcessor.SendAsync(request, cancellationToken);

        return Results.NoContent();
    }

    public string GroupName => PostsConfigs.Tag;
    public string PrefixRoute => PostsConfigs.PostPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/RemoveReaction", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("RemoveReaction")
            .WithDisplayName("Remove Reaction.");
    }
}

public class RemoveReactionHandler : ICommandHandler<RemoveReaction>
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

    public async Task<Unit> Handle(RemoveReaction request, CancellationToken cancellationToken)
    {
        PostId postId = PostId.Of(request.Id);
        await _reactionService.RemoveReactionAsync<Post, PostId>(
            PostId.Of(request.Id),
            _currentUserService.YtsooberId,
            cancellationToken
        );
        await _ytsooberReactionCache.RemoveCache(
            postId.ToString(),
            _currentUserService.YtsooberId,
            typeof(Post).ToString()
        );
        return Unit.Value;
    }
}

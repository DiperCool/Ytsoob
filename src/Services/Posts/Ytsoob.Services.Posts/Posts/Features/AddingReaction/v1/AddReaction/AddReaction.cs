using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Reactions.Enums;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Posts.Features.AddingReaction.v1.AddReaction;

public record AddReaction(long Id, ReactionType ReactionType) : ITxCreateCommand;

public class AddReactionEndpoint : ICommandMinimalEndpoint<AddReaction>
{
    public async Task<IResult> HandleAsync(
        HttpContext context,
        AddReaction request,
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
            .MapPost("/AddReaction", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("AddReaction")
            .WithDisplayName("Add Reaction.");
    }
}

public class AddReactionHandler : ICommandHandler<AddReaction>
{
    private IReactionService _reactionService;
    private ICurrentUserService _currentUserService;

    public AddReactionHandler(IReactionService reactionService, ICurrentUserService currentUserService)
    {
        _reactionService = reactionService;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(AddReaction request, CancellationToken cancellationToken)
    {
        await _reactionService.AddReactionAsync<Post, PostId>(
            PostId.Of(request.Id),
            _currentUserService.YtsooberId,
            request.ReactionType,
            cancellationToken
        );
        return Unit.Value;
    }
}

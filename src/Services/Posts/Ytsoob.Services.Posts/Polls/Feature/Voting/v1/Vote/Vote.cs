using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.CQRS.Events.Internal;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Exceptions;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Polls.ValueObjects;
using Ytsoob.Services.Posts.Posts;
using Ytsoob.Services.Posts.Posts.Exception;
using Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Polls.Feature.Voting.v1.Vote;

public record Vote(long PostId, long OptionId) : ITxCreateCommand;

public class VoteEndpoint : ICommandMinimalEndpoint<Vote>
{
    public string GroupName => PollConfigs.Tag;
    public string PrefixRoute => PollConfigs.PollPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/vote", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("Vote")
            .WithDisplayName("Vote.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] Vote request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(VoteEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("PostId", request.PostId))
        {
            var result = await commandProcessor.SendAsync(request, cancellationToken);

            return Results.Ok(result);
        }
    }
}

public class VoteHandler : ICommandHandler<Vote>
{
    private IPostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;

    public VoteHandler(IPostsDbContext postsDbContext, ICurrentUserService currentUserService)
    {
        _postsDbContext = postsDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(Vote request, CancellationToken cancellationToken)
    {
        Post? post = await _postsDbContext.Posts
            .Include(x => x.Poll)
            .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken);
        if (post == null)
            throw new PostNotFoundException(request.PostId);

        Option? option = await _postsDbContext.Options.FirstOrDefaultAsync(
            x => x.Id == request.OptionId,
            cancellationToken: cancellationToken
        );
        if (option == null)
            throw new OptionNotFoundException(request.OptionId);
        Ytsoober? ytsoober = await _postsDbContext.Ytsoobers.FirstOrDefaultAsync(
            x => x.Id == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (ytsoober == null)
            throw new YtsooberNotFoundException(_currentUserService.YtsooberId);
        post.VotePoll(ytsoober, option);
        _postsDbContext.Posts.Update(post);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

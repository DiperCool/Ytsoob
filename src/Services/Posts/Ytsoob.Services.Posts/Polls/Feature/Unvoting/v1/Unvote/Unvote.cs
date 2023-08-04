using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Exceptions;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Posts.Exception;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Shared.Contracts;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Polls.Feature.Unvoting.v1.Unvote;

public record Unvote(long PostId, long OptionId) : ITxUpdateCommand;

public class UnvoteEndpoint : ICommandMinimalEndpoint<Unvote>
{
    public async Task<IResult> HandleAsync(
        HttpContext context,
        Unvote request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        var result = await commandProcessor.SendAsync(request, cancellationToken);
        return Results.Ok(result);
    }

    public string GroupName => PollConfigs.Tag;
    public string PrefixRoute => PollConfigs.PollPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/unvote", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("Unvote")
            .WithDisplayName("Unvote.");
    }
}

public class UnvoteHandler : ICommandHandler<Unvote>
{
    private IPostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;

    public UnvoteHandler(IPostsDbContext postsDbContext, ICurrentUserService currentUserService)
    {
        _postsDbContext = postsDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(Unvote request, CancellationToken cancellationToken)
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
        post.UnvotePoll(ytsoober, option);
        return Unit.Value;
    }
}

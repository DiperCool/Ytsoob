using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Comments.Models;
using Ytsoob.Services.Posts.Comments.ValueObjects;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Comments.Features.GettingRepliedComments.v1.GetRepliedComments;

public record GetRepliedCommentsResponse(ListResultModel<CommentDto> Result);

public record GetRepliedComments(long PostId) : ListQuery<GetRepliedCommentsResponse>;

public class GetRepliedCommentsEndpoint : IMinimalEndpoint
{
    public string GroupName => CommentsConfigs.Tag;
    public string PrefixRoute => CommentsConfigs.CommentPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet("/GetRepliedComments", HandleAsync)
            .Produces<GetRepliedCommentsResponse>(StatusCodes.Status200OK)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("GetRepliedComments")
            .WithDisplayName("Get Replied Comments.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromQuery] long commentId,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        IQueryProcessor queryProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        GetRepliedCommentsResponse result = await queryProcessor.SendAsync(
            new GetRepliedComments(commentId) { Page = page, PageSize = pageSize },
            cancellationToken
        );

        return Results.Ok(result);
    }
}

public class GetRepliedCommentsValidator : AbstractValidator<GetRepliedComments>
{
    public GetRepliedCommentsValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public class GetRepliedCommentsHandler : IQueryHandler<GetRepliedComments, GetRepliedCommentsResponse>
{
    private IPostsDbContext _postsDbContext;
    private IMapper _mapper;
    private ICurrentUserService _currentUserService;
    private ICacheYtsooberReaction _cacheYtsooberReaction;

    public GetRepliedCommentsHandler(
        ICurrentUserService currentUserService,
        IMapper mapper,
        IPostsDbContext postsDbContext,
        ICacheYtsooberReaction cacheYtsooberReaction
    )
    {
        _currentUserService = currentUserService;
        _mapper = mapper;
        _postsDbContext = postsDbContext;
        _cacheYtsooberReaction = cacheYtsooberReaction;
    }

    public async Task<GetRepliedCommentsResponse> Handle(
        GetRepliedComments request,
        CancellationToken cancellationToken
    )
    {
        var comments = await _postsDbContext.RepliedComments
            .Include(x => x.ReactionStats)
            .Where(x => x.CommentId == request.PostId)
            .OrderBy(x => x.Created)
            .AsNoTracking()
            .ApplyPagingAsync<RepliedComment, CommentDto>(
                _mapper.ConfigurationProvider,
                request.Page,
                request.PageSize,
                cancellationToken: cancellationToken
            );
        if (_currentUserService.IsAuthenticated)
        {
            await CalculateUserReaction(comments);
        }

        return new GetRepliedCommentsResponse(comments);
    }

    private async Task CalculateUserReaction(ListResultModel<CommentDto> comments)
    {
        foreach (CommentDto comment in comments.Items)
        {
            comment.YtsooberReaction = await _cacheYtsooberReaction.GetYtsooberReactionAsync(
                CommentId.Of(comment.Id).ToString(),
                _currentUserService.YtsooberId,
                typeof(BaseComment).ToString()
            );
        }
    }
}

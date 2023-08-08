using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.IdsGenerator;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Comments.Exceptions;
using Ytsoob.Services.Posts.Comments.Models;
using Ytsoob.Services.Posts.Comments.ValueObjects;
using Ytsoob.Services.Posts.Posts.Exception;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Reactions.Models;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Comments.Features.ReplyComment.v1.ReplyComment;

public record ReplyComment(long PostId, long CommentId, long ReplyToCommentId, string Content)
    : ITxCreateCommand<CommentDto>;

public class ReplyCommentValidator : AbstractValidator<ReplyComment>
{
    public ReplyCommentValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(150);
    }
}

public class ReplyCommentEndpoint : ICommandMinimalEndpoint<ReplyComment>
{
    public string GroupName => CommentsConfigs.Tag;
    public string PrefixRoute => CommentsConfigs.PostPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/Reply", HandleAsync)
            .RequireAuthorization()
            .Produces<CommentDto>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("ReplyComment")
            .WithDisplayName("Reply Comments.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] ReplyComment request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(ReplyCommentEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("CommentId", request.ReplyToCommentId))
        {
            var result = await commandProcessor.SendAsync(request, cancellationToken);

            return Results.Ok(result);
        }
    }
}

public class ReplyCommentHandler : ICommandHandler<ReplyComment, CommentDto>
{
    private IPostsDbContext _postsDbContext;
    private IMapper _mapper;

    public ReplyCommentHandler(IPostsDbContext postsDbContext, IMapper mapper)
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(ReplyComment request, CancellationToken cancellationToken)
    {
        if (!await _postsDbContext.Posts.AnyAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken))
        {
            throw new PostNotFoundException(request.PostId);
        }

        if (
            !await _postsDbContext.Comments.AnyAsync(
                x => x.Id == request.CommentId && x.PostId == request.PostId,
                cancellationToken: cancellationToken
            )
        )
        {
            throw new CommentNotFoundException(request.CommentId);
        }

        bool replyToRepliedComment = request.CommentId != request.ReplyToCommentId;
        bool repliedCommentExistInComment = !await _postsDbContext.RepliedComments.AnyAsync(
            x => x.CommentId == request.CommentId && x.Id == request.ReplyToCommentId,
            cancellationToken: cancellationToken
        );
        if (replyToRepliedComment && repliedCommentExistInComment)
        {
            throw new CommentNotFoundException(request.ReplyToCommentId);
        }

        RepliedComment comment = RepliedComment.Create(
            CommentId.Of(SnowFlakIdGenerator.NewId()),
            CommentId.Of(request.CommentId),
            PostId.Of(request.PostId),
            CommentId.Of(request.ReplyToCommentId),
            CommentContent.Of(request.Content),
            ReactionStats.Create(SnowFlakIdGenerator.NewId())
        );
        await _postsDbContext.BaseComments.AddAsync(comment, cancellationToken);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<RepliedComment, CommentDto>(comment);
    }
}

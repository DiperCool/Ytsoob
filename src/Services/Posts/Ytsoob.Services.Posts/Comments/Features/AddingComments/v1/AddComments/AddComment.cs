using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.IdsGenerator;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Ytsoob.Services.Posts.Comments.Models;
using Ytsoob.Services.Posts.Comments.ValueObjects;
using Ytsoob.Services.Posts.Posts;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Reactions.Models;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Comments.AddingComments.v1.AddComments;

public record AddComment(long PostId, string Content) : ITxCreateCommand<CommentDto>
{
    public CommentId CommentId = CommentId.Of(SnowFlakIdGenerator.NewId());
}

public class AddCommentValidator : AbstractValidator<AddComment>
{
    public AddCommentValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(150);
    }
}

public class AddCommentEndpoint : ICommandMinimalEndpoint<AddComment>
{
    public string GroupName => CommentsConfigs.Tag;
    public string PrefixRoute => CommentsConfigs.PostPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/Add", HandleAsync)
            .RequireAuthorization()
            .Produces<CommentDto>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("AddComment")
            .WithDisplayName("Add Comments.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] AddComment request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(AddCommentEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("PostId", request.PostId))
        {
            var result = await commandProcessor.SendAsync(request, cancellationToken);

            return Results.Ok(result);
        }
    }
}

public class AddCommentHandler : ICommandHandler<AddComment, CommentDto>
{
    private IPostsDbContext _postsDbContext;
    private IMapper _mapper;

    public AddCommentHandler(IPostsDbContext postsDbContext, IMapper mapper)
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(AddComment request, CancellationToken cancellationToken)
    {
        Comment comment = Comment.Create(
            request.CommentId,
            PostId.Of(request.PostId),
            CommentContent.Of(request.Content),
            ReactionStats.Create(SnowFlakIdGenerator.NewId())
        );
        await _postsDbContext.BaseComments.AddAsync(comment, cancellationToken);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<Comment, CommentDto>(comment);
    }
}

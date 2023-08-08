using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Comments.Exceptions;
using Ytsoob.Services.Posts.Comments.Models;
using Ytsoob.Services.Posts.Comments.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Comments.Features.UpdatingComment.v1.UpdateComment;

public record UpdateComment(long CommentId, string Content) : ITxUpdateCommand;

public class UpdateCommentValidator : AbstractValidator<UpdateComment>
{
    public UpdateCommentValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(150);
    }
}

public class UpdateCommentEndpoint : ICommandMinimalEndpoint<UpdateComment>
{
    public string GroupName => CommentsConfigs.Tag;
    public string PrefixRoute => CommentsConfigs.CommentPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPut("/Update", HandleAsync)
            .RequireAuthorization()
            .Produces<CommentDto>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("UpdateComment")
            .WithDisplayName("Update Comment.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] UpdateComment request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(UpdateCommentEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("CommentId", request.CommentId))
        {
            var result = await commandProcessor.SendAsync(request, cancellationToken);

            return Results.Ok(result);
        }
    }
}

public class UpdateCommentHandler : ICommandHandler<UpdateComment>
{
    private IPostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;

    public UpdateCommentHandler(ICurrentUserService currentUserService, IPostsDbContext postsDbContext)
    {
        _currentUserService = currentUserService;
        _postsDbContext = postsDbContext;
    }

    public async Task<Unit> Handle(UpdateComment request, CancellationToken cancellationToken)
    {
        Comment? comment = await _postsDbContext.Comments.FirstOrDefaultAsync(
            x => x.Id == request.CommentId && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (comment == null)
        {
            throw new CommentNotFoundException(request.CommentId);
        }

        comment.UpdateContent(CommentContent.Of(request.Content));
        _postsDbContext.Comments.Update(comment);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

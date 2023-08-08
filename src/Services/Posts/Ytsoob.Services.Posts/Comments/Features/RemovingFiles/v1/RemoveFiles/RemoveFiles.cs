using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Comments.Exceptions;
using Ytsoob.Services.Posts.Comments.Models;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Comments.Features.RemovingFiles.v1.RemoveFiles;

public record RemoveFiles(long CommentId, IEnumerable<string> Files) : ITxUpdateCommand;

public class RemoveFilesEndpoint : ICommandMinimalEndpoint<RemoveFiles>
{
    public string GroupName => CommentsConfigs.Tag;
    public string PrefixRoute => CommentsConfigs.CommentPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete("/RemoveFiles", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("RemoveFilesComment")
            .WithDisplayName("Remove Files.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] RemoveFiles request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(RemoveFilesEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("CommentId", request.CommentId))
        {
            var result = await commandProcessor.SendAsync(request, cancellationToken);

            return Results.NoContent();
        }
    }
}

public class RemoveFilesHandler : ICommandHandler<RemoveFiles>
{
    private IPostsDbContext _postsDbContext;
    private ICommentFilesBlobStorage _blob;
    private ICurrentUserService _currentUserService;

    public RemoveFilesHandler(
        ICurrentUserService currentUserService,
        ICommentFilesBlobStorage blob,
        IPostsDbContext postsDbContext
    )
    {
        _currentUserService = currentUserService;
        _blob = blob;
        _postsDbContext = postsDbContext;
    }

    public async Task<Unit> Handle(RemoveFiles request, CancellationToken cancellationToken)
    {
        BaseComment? comment = await _postsDbContext.BaseComments.FirstOrDefaultAsync(
            x => x.Id == request.CommentId && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (comment == null)
        {
            throw new CommentNotFoundException(request.CommentId);
        }

        foreach (var file in request.Files)
        {
            comment.RemoveFile(file);
        }

        _postsDbContext.BaseComments.Update(comment);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

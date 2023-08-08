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
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Comments.Features.AddingFiles.v1.AddFiles;

public record AddFiles(long CommentId, List<IFormFile> Files) : ITxUpdateCommand;

public class AddFilesValidator : AbstractValidator<AddFiles>
{
    public AddFilesValidator()
    {
        RuleFor(x => x.Files).Must(x => x.Count < 10).WithMessage("Exceed limit of files");
    }
}

public class AddFilesEndpoint : IMinimalEndpoint
{
    public string GroupName => CommentsConfigs.Tag;
    public string PrefixRoute => CommentsConfigs.CommentPrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/AddFiles", HandleAsync)
            .RequireAuthorization()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("AddFilesComment")
            .WithDisplayName("Add files.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromQuery] long commentId,
        [FromForm] IFormFileCollection files,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(commentId, nameof(commentId));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(AddFilesEndpoint)))
        using (Serilog.Context.LogContext.PushProperty("CommentId", commentId))
        {
            var result = await commandProcessor.SendAsync(new AddFiles(commentId, files.ToList()), cancellationToken);

            return Results.NoContent();
        }
    }
}

public class AddFilesHandler : ICommandHandler<AddFiles>
{
    private ICommentFilesBlobStorage _blob;
    private ICurrentUserService _currentUserService;
    private IPostsDbContext _postsDbContext;

    public AddFilesHandler(
        IPostsDbContext postsDbContext,
        ICurrentUserService currentUserService,
        ICommentFilesBlobStorage blob
    )
    {
        _postsDbContext = postsDbContext;
        _currentUserService = currentUserService;
        _blob = blob;
    }

    public async Task<Unit> Handle(AddFiles request, CancellationToken cancellationToken)
    {
        BaseComment? comment = await _postsDbContext.BaseComments.FirstOrDefaultAsync(
            x => x.Id == request.CommentId && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (comment == null)
        {
            throw new CommentNotFoundException(request.CommentId);
        }

        IEnumerable<string?> files = await _blob.UploadFilesAsync(request.Files, cancellationToken);
        foreach (var file in files)
        {
            if (file != null)
                comment.AddFile(file);
        }

        _postsDbContext.BaseComments.Update(comment);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

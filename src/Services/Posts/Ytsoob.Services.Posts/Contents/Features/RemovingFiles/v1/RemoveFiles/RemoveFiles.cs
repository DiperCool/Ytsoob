using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Contents.Features.UpdatingPostContent.v1;
using Ytsoob.Services.Posts.Posts.Exception;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Contents.Features.RemovingFiles.v1.RemoveFiles;

public record RemoveFiles(long PostId, IEnumerable<string> Files) : ICommand;

public class RemoveFileEndpoint : IMinimalEndpoint
{
    public string GroupName => ContentsConfig.Tag;
    public string PrefixRoute => ContentsConfig.PostPrefixUri;
    public double Version => 1.0;
    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete("/", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("AddFiles")
            .WithDisplayName("Add files.");
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] AddFiles command,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(command, nameof(command));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(RemoveFiles)))
            using (Serilog.Context.LogContext.PushProperty("PostId", command.PostId))
            {
                var result = await commandProcessor.SendAsync(command, cancellationToken);

                return Results.Ok(result);
            }
    }
}

public class RemoveFilesHandler : ICommandHandler<RemoveFiles>
{
    private IPostsDbContext _context;
    private ICurrentUserService _currentUserService;
    private IContentBlobStorage _contentBlobStorage;

    public RemoveFilesHandler(IPostsDbContext context, ICurrentUserService currentUserService, IContentBlobStorage contentBlobStorage)
    {
        _context = context;
        _currentUserService = currentUserService;
        _contentBlobStorage = contentBlobStorage;
    }

    public async Task<Unit> Handle(RemoveFiles request, CancellationToken cancellationToken)
    {
        Post? post = await _context.Posts
                         .Include(x => x.Content)
                         .Where(x => x.CreatedBy == _currentUserService.YtsooberId && x.Id == request.PostId)
                         .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (post == null) throw new PostNotFoundException(request.PostId);
        foreach (string file in request.Files)
        {
            post.RemoveFileFromContent(file);
        }

        await _contentBlobStorage.RemoveFilesAsync(request.Files, cancellationToken);
        return Unit.Value;
    }
}

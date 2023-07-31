using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.Exception.Types;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ytsoob.Services.Posts.Posts.Exception;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Contents.Features.UpdatingPostContent.v1;


public record AddFiles(long PostId, List<IFormFile> Files) : ITxUpdateCommand;

public class AddFilesEndpoint : IMinimalEndpoint
{
    public string GroupName => ContentsConfig.Tag;
    public string PrefixRoute => ContentsConfig.PostPrefixUri;
    public double Version => 1.0;
    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPost("/", HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName("AddFiles")
            .WithDisplayName("Add files.");
    }
    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromQuery] long postId,
        [FromForm] IFormFileCollection files,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(postId, nameof(postId));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(UpdatePostContentEndpoint)))
            using (Serilog.Context.LogContext.PushProperty("PostId", postId))
            {
                var result = await commandProcessor.SendAsync(new AddFiles(postId, files.ToList()), cancellationToken);

                return Results.Ok(result);
            }
    }
}

public class AddFilesHandler : ICommandHandler<AddFiles>
{
    private IContentBlobStorage _contentBlobStorage;
    private IPostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;
    private IMapper _mapper;

    public AddFilesHandler(IContentBlobStorage contentBlobStorage, IPostsDbContext postsDbContext, IMapper mapper, ICurrentUserService currentUserService)
    {
        _contentBlobStorage = contentBlobStorage;
        _postsDbContext = postsDbContext;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(AddFiles request, CancellationToken cancellationToken)
    {
        Post? post = await _postsDbContext.Posts
                         .Include(x => x.Content)
                         .Where(x => x.CreatedBy == _currentUserService.YtsooberId && x.Id == request.PostId)
                         .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (post == null) throw new PostNotFoundException(request.PostId);
        if (!request.Files.Any()) throw new BadRequestException("Files empty");
        IEnumerable<string?> files = await _contentBlobStorage.UploadFilesAsync(request.Files, cancellationToken);
        foreach (var file in files)
        {
            if (file != null) post.AddFile(file);
        }

        _postsDbContext.Posts.Update(post);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

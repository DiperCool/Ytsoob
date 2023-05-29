using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Core.Exception;
using BuildingBlocks.Security.Jwt;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Posts.Exception;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Posts.Features.DeletingPost;

public record DeletePostCommand(PostId PostId) : ITxCommand<Unit>;

public class DeletePostHandler : ICommandHandler<DeletePostCommand, Unit>
{
    private IPostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;
    public DeletePostHandler(IPostsDbContext postsDbContext, ICurrentUserService currentUserService)
    {
        _postsDbContext = postsDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var post = await _postsDbContext.Posts
            .Include(x => x.Content)
            .FirstOrDefaultAsync(x => x.Id == request.PostId && x.CreatedBy == _currentUserService.UserIdGuid, cancellationToken: cancellationToken);
        Guard.Against.NotFound(post, new PostNotFoundException(request.PostId));
        post!.Delete();
        _postsDbContext.Posts.Remove(post);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}

using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Contents.ValueObjects;
using Ytsoob.Services.Posts.Posts.Exception;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Contents.Features.UpdatingPostContent.v1;

public record UpdatePostContent(PostId PostId, string ContentText) : ITxUpdateCommand<UpdatePostContentResponse>;


public class UpdateTextPostValidator : AbstractValidator<UpdatePostContent>
{
    public UpdateTextPostValidator()
    {
        RuleFor(x => x.ContentText).NotEmpty().NotNull();
    }
}


public class UpdateTextPostHandler : ICommandHandler<UpdatePostContent, UpdatePostContentResponse>
{
    private IPostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;

    public UpdateTextPostHandler(IPostsDbContext postsDbContext, ICurrentUserService currentUserService)
    {
        _postsDbContext = postsDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<UpdatePostContentResponse> Handle(UpdatePostContent request, CancellationToken cancellationToken)
    {
        Post? post = await _postsDbContext.Posts
            .Include(x => x.Content)
            .FirstOrDefaultAsync(x => x.Id == request.PostId && x.CreatedBy == _currentUserService.YtsooberId, cancellationToken: cancellationToken);
        if (post == null)
        {
            throw new PostNotFoundException(request.PostId);
        }

        post.UpdateContentText(ContentText.Of(request.ContentText));
        _postsDbContext.Posts.Update(post);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return new UpdatePostContentResponse();
    }
}

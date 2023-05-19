using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Core.Exception.Types;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Contents.ValueObjects;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Posts.Features.UpdatingTextPost.v1;

public record UpdateTextPost(PostId PostId, string ContentText) : ITxUpdateCommand<UpdateTextPostResponse>;


public class UpdateTextPostValidator : AbstractValidator<UpdateTextPost>
{
    public UpdateTextPostValidator()
    {
        RuleFor(x => x.ContentText).NotEmpty().NotNull();
    }
}


public class UpdateTextPostHandler : ICommandHandler<UpdateTextPost, UpdateTextPostResponse>
{
    private IPostsDbContext _postsDbContext;
    private ISecurityContextAccessor _securityContextAccessor;

    public UpdateTextPostHandler(IPostsDbContext postsDbContext, ISecurityContextAccessor securityContextAccessor)
    {
        _postsDbContext = postsDbContext;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<UpdateTextPostResponse> Handle(UpdateTextPost request, CancellationToken cancellationToken)
    {
        Post? post = await _postsDbContext.Posts
            .Include(x => x.Content)
            .FirstOrDefaultAsync(x => x.Id == request.PostId && x.CreatedBy == _securityContextAccessor.UserIdGuid, cancellationToken: cancellationToken);
        if (post == null)
        {
            throw new NotFoundException("Post with this Id not found");
        }

        post.UpdateContentText(ContentText.Of(request.ContentText));
        return new UpdateTextPostResponse();
    }
}

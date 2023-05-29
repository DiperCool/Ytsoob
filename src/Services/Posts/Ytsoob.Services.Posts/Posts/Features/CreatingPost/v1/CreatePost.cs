using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Core.IdsGenerator;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Ytsoob.Services.Posts.Contents.Models;
using Ytsoob.Services.Posts.Contents.ValueObjects;
using Ytsoob.Services.Posts.Posts.Dtos;
using Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1.Request;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1;

public record CreatePost(
        ContentRequest Content
    )
    : ITxCreateCommand<CreatePostResponse>
{
    public long Id { get; init; } = SnowFlakIdGenerator.NewId();
}

public class CreatePostValidator : AbstractValidator<CreatePost>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.Content).NotNull();
        RuleFor(x => x.Content.ContentText).NotEmpty().NotNull();
    }
}


public class CreatePostHandler : ICommandHandler<CreatePost, CreatePostResponse>
{
    private IPostsDbContext _postsDbContext;
    private IMapper _mapper;
    private ICurrentUserService _currentUserService;
    public CreatePostHandler(IPostsDbContext postsDbContext, IMapper mapper, ICurrentUserService currentUserService)
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<CreatePostResponse> Handle(CreatePost request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var content = new Content(ContentId.Of(SnowFlakIdGenerator.NewId()), ContentText.Of(request.Content.ContentText));
        var post = Post.Create(PostId.Of(request.Id), content);
        await _postsDbContext.Posts.AddAsync(post, cancellationToken);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return new CreatePostResponse(_mapper.Map<PostDto>(post));
    }
}

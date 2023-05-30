using BuildingBlocks.Abstractions.Domain;
using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Posts.Contents.Models;
using Ytsoob.Services.Posts.Contents.ValueObjects;
using Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1.Events;
using Ytsoob.Services.Posts.Posts.Features.DeletingPost;
using Ytsoob.Services.Posts.Posts.Features.UpdatingTextPost.v1.Events;
using Ytsoob.Services.Posts.Posts.ValueObjects;

namespace Ytsoob.Services.Posts.Posts.Models;

public class Post : Aggregate<PostId>
{
    public Content Content { get; private set; } = default!;

    public Post(PostId postId, Content content)
    {
        Id = postId;
        Content = content;
    }

    // ef
    protected Post()
    {
    }

    public void UpdateContentText(ContentText contentText)
    {
        Content.UpdateText(contentText);
        AddDomainEvents(new PostContentUpdated(Id, contentText));
    }

    public static Post Create(PostId postId, Content content)
    {
        Post post = new Post() { Id = postId, Content = content };
        post.AddDomainEvents(new PostCreated(post));
        return post;
    }

    public void Delete()
    {
        AddDomainEvents(new PostDeleted(this));
    }

    public DateTime? LastModified { get; } = null;
    public int? LastModifiedBy { get; } = null;
}

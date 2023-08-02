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
    public Poll.Models.Poll? Poll { get; private set; }

    public Post(PostId postId, Content content)
    {
        Id = postId;
        Content = content;
    }

    // ef
    protected Post() { }

    public void UpdateContentText(ContentText contentText)
    {
        Content.UpdateText(contentText);
        AddDomainEvents(new PostContentUpdated(Id, contentText, Content.Files));
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

    public void AddFileToContent(string fileUrl)
    {
        Content.AddFile(fileUrl);
        AddDomainEvents(new PostContentUpdated(Id, Content.ContentText, Content.Files));
    }

    public void RemoveFileFromContent(string fileUrl)
    {
        Content.RemoveFile(fileUrl);
        AddDomainEvents(new PostContentUpdated(Id, Content.ContentText, Content.Files));
    }
}

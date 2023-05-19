using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Posts.Contents.Models;
using Ytsoob.Services.Posts.Contents.ValueObjects;
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

    public Post()
    {
    }

    public void UpdateContentText(ContentText contentText)
    {
        Content.UpdateText(contentText);
    }
}

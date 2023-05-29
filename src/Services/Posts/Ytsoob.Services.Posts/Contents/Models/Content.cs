using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Posts.Contents.Exceptions.Domain;
using Ytsoob.Services.Posts.Contents.ValueObjects;
using Ytsoob.Services.Posts.Posts.ValueObjects;

namespace Ytsoob.Services.Posts.Contents.Models;

public class Content : Entity<ContentId>
{
    public ContentText ContentText { get; private set; } = default!;
    public PostId PostId { get; private set; } = default!;

    public Content(ContentId contentId, ContentText contentText)
    {
        Id = contentId;
        UpdateText(contentText);
    }

    protected Content()
    {
    }

    public void UpdateText(ContentText text)
    {
        ContentText = text;
    }
}

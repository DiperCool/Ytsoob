using Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1.Request;

namespace Ytsoob.Services.Posts.Posts.Features.CreatingPost.v1;

public class CreatePostRequest
{
    public ContentRequest Content { get; set; } = default!;
    public PollRequest? Poll { get; set; }
}

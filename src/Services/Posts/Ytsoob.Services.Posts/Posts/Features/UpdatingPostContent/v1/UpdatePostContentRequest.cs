namespace Ytsoob.Services.Posts.Posts.Features.UpdatingTextPost.v1;

public class UpdatePostContentRequest
{
    public long PostId { get; set; }
    public string ContentText { get; set; } = string.Empty;
}

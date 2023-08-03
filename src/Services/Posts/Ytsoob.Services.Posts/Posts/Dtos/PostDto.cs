using Ytsoob.Services.Posts.Contents.Dtos;
using Ytsoob.Services.Posts.Polls.Dtos;

namespace Ytsoob.Services.Posts.Posts.Dtos;

public class PostDto
{
    public long Id { get; set; }
    public ContentDto? Content { get; set; }
    public PollDto? Poll { get; set; }
}

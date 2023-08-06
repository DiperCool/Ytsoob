using Ytsoob.Services.Posts.Contents.Dtos;
using Ytsoob.Services.Posts.Polls.Dtos;
using Ytsoob.Services.Posts.Reactions.Dtos;

namespace Ytsoob.Services.Posts.Posts.Dtos;

public class PostDto
{
    public long Id { get; set; }
    public ContentDto? Content { get; set; }
    public PollDto? Poll { get; set; }
    public ReactionStatsDto ReactionStats { get; set; } = default!;
}

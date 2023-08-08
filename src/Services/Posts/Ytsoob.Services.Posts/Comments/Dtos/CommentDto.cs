using Ytsoob.Services.Posts.Reactions.Dtos;
using Ytsoob.Services.Posts.Reactions.Enums;

namespace Ytsoob.Services.Posts;

public class CommentDto
{
    public long Id { get; set; }
    public string Content { get; set; } = default!;
    public ReactionStatsDto ReactionStats { get; private set; } = default!;
    public IList<string> Files { get; private set; } = new List<string>();
    public long PostId { get; set; }
    public ReactionType? YtsooberReaction { get; set; }
}

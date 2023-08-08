using Ytsoob.Services.Posts.Comments.ValueObjects;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Reactions.Models;

namespace Ytsoob.Services.Posts.Comments.Models;

public class Comment : BaseComment
{
    // ef core
    protected Comment() { }

    protected Comment(CommentId commentId, PostId postId, CommentContent content, ReactionStats reactionStats)
        : base(commentId, postId, content, reactionStats) { }

    public IList<RepliedComment> RepliedComments { get; set; } = new List<RepliedComment>();

    public static Comment Create(
        CommentId commentId,
        PostId postId,
        CommentContent content,
        ReactionStats reactionStats
    )
    {
        return new Comment(commentId, postId, content, reactionStats);
    }
}

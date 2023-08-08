using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Posts.Comments.ValueObjects;
using Ytsoob.Services.Posts.Posts.Exception;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Reactions.Enums;
using Ytsoob.Services.Posts.Reactions.Models;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Comments.Models;

public abstract class BaseComment : Aggregate<CommentId>, IEntityWithReactions<CommentId>
{
    public CommentContent Content { get; set; } = default!;
    public ReactionStats ReactionStats { get; private set; } = default!;
    public List<string> Files { get; private set; } = default!;
    public PostId PostId { get; set; }

    // ef core
    protected BaseComment() { }

    protected BaseComment(CommentId commentId, PostId postId, CommentContent content, ReactionStats reactionStats)
    {
        Id = commentId;
        Content = content;
        PostId = postId;
        ReactionStats = reactionStats;
        Files = new List<string>();
    }

    public void AddReaction(ReactionType reactionType)
    {
        ReactionStats.AddReaction(reactionType);
    }

    public void RemoveReaction(ReactionType reactionType)
    {
        ReactionStats.RemoveReaction(reactionType);
    }

    public void AddFile(string file)
    {
        Files.Add(file);
    }

    public void RemoveFile(string file)
    {
        if (!Files.Contains(file))
        {
            throw new FileNotFound(file);
        }

        Files.Remove(file);
    }

    public void UpdateContent(CommentContent content)
    {
        Content = content;
    }
}

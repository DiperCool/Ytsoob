using Ytsoob.Services.Posts.Reactions.Enums;
using Ytsoob.Services.Posts.Reactions.Models;

namespace Ytsoob.Services.Posts.Shared.Contracts;

public interface IEntityWithReactions<TId>
{
    public TId Id { get; }
    public ReactionStats ReactionStats { get; }

    public void AddReaction(ReactionType reactionType);
    public void RemoveReaction(ReactionType reactionType);
}

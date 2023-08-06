using Ytsoob.Services.Posts.Reactions.Enums;

namespace Ytsoob.Services.Posts.Shared.Contracts;

public interface IReactionService
{
    public Task AddReactionAsync<T, TId>(
        TId entityId,
        long ytsooberId,
        ReactionType reactionType,
        CancellationToken cancellationToken = default
    )
        where T : class, IEntityWithReactions<TId>;
    public Task RemoveReactionAsync<T, TId>(
        TId entityId,
        long ytsooberId,
        CancellationToken cancellationToken = default
    )
        where T : class, IEntityWithReactions<TId>;
}

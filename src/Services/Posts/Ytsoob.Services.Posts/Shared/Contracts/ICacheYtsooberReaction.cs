using Ytsoob.Services.Posts.Reactions.Enums;

namespace Ytsoob.Services.Posts.Shared.Contracts;

public interface ICacheYtsooberReaction
{
    public Task<ReactionType?> GetYtsooberReactionAsync(string entityId, long ytsooberId, string entityType);
    public Task RemoveCache(string entityId, long ytsooberId, string entityType);
}

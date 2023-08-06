using System.Globalization;
using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Reactions;
using Ytsoob.Services.Posts.Reactions.Enums;
using Ytsoob.Services.Posts.Reactions.Models;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Shared.Services;

public class CacheYtsooberReaction : ICacheYtsooberReaction
{
    private IEasyCachingProvider _cache;
    private IPostsDbContext _postsDbContext;

    public CacheYtsooberReaction(IEasyCachingProvider cache, IPostsDbContext postsDbContext)
    {
        _cache = cache;
        _postsDbContext = postsDbContext;
    }

    public async Task<ReactionType?> GetYtsooberReactionAsync(string entityId, long ytsooberId, string entityType)
    {
        string key = string.Format(
            CultureInfo.InvariantCulture,
            ReactionCacheKeys.UserReactionCacheKey,
            entityId,
            ytsooberId,
            entityType
        );

        if (!await _cache.ExistsAsync(key))
        {
            Reaction? reaction = await _postsDbContext.Reactions
                .Where(x => x.EntityId == entityId && x.YtsooberId == ytsooberId)
                .FirstOrDefaultAsync();
            ReactionType? reactionType = reaction?.ReactionType;
            string? reactionTypeStr = reactionType != null ? reactionType.ToString() : string.Empty;
            await _cache.SetAsync(key, reactionTypeStr, TimeSpan.FromMinutes(60));
            return reactionType;
        }

        var cache = await _cache.GetAsync<string>(key);
        string value = cache.Value;
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        return Enum.TryParse(value, true, out ReactionType result) ? result : null;
    }

    public async Task RemoveCache(string entityId, long ytsooberId, string entityType)
    {
        string key = string.Format(
            CultureInfo.InvariantCulture,
            ReactionCacheKeys.UserReactionCacheKey,
            entityId,
            ytsooberId,
            entityType
        );
        await _cache.RemoveAsync(key);
    }
}

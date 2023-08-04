using System.Globalization;
using EasyCaching.Core;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Ytsoob.Services.Posts.Polls;
using Ytsoob.Services.Posts.Polls.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Shared.Services;

public class CacheYtsooberOptions : ICacheYtsooberOptions
{
    private IEasyCachingProvider _cache;
    private IPostsDbContext _postsDbContext;

    public CacheYtsooberOptions(IEasyCachingProvider cache, IPostsDbContext postsDbContext)
    {
        _cache = cache;
        _postsDbContext = postsDbContext;
    }

    public async Task<IEnumerable<long>> GetUsersOptionsInPollAsync(long pollId, long ytsooberId)
    {
        string key = string.Format(
            CultureInfo.InvariantCulture,
            PollCacheKeys.UserPollVotedCacheKey,
            ytsooberId,
            pollId
        );

        if (await _cache.ExistsAsync(key))
        {
            var cache = await _cache.GetAsync<IEnumerable<long>>(key);
            return cache.Value;
        }

        var optionIds = await _postsDbContext.Voters
            .Where(x => x.Option.PollId == pollId)
            .Select(x => x.OptionId)
            .Select(x => x.Value)
            .ToListAsync();
        await _cache.SetAsync(key, optionIds, TimeSpan.FromMinutes(60));
        return optionIds;
    }
}

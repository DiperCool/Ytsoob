using System.Globalization;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using EasyCaching.Core;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Polls.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Polls.Feature.Voting.v1.Vote.Events;

public record VoterVotedDomainEvent(Poll Poll, OptionId OptionId, long YtsooberId) : DomainEvent;

public class Vote : IDomainEventHandler<VoterVotedDomainEvent>
{
    private IPostsDbContext _postsDbContext;
    private IEnumerable<IPollStrategy> _pollStrategies;
    private IEasyCachingProvider _cache;

    public Vote(IPostsDbContext postsDbContext, IEnumerable<IPollStrategy> pollStrategies, IEasyCachingProvider cache)
    {
        _postsDbContext = postsDbContext;
        _pollStrategies = pollStrategies;
        _cache = cache;
    }

    public async Task Handle(VoterVotedDomainEvent notification, CancellationToken cancellationToken)
    {
        IPollStrategy pollStrategy = _pollStrategies.First(x => x.Check(notification.Poll.PollAnswerType));
        await pollStrategy.Vote(notification.Poll, notification.OptionId, notification.YtsooberId);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        string key = string.Format(
            CultureInfo.InvariantCulture,
            PollCacheKeys.UserPollVotedCacheKey,
            notification.YtsooberId,
            notification.Poll.Id.Value
        );
        await _cache.RemoveAsync(key, cancellationToken);
    }
}

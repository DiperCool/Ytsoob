using System.Globalization;
using BuildingBlocks.Core.IdsGenerator;
using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Ytsoob.Services.Posts.Exceptions.Domains;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Polls.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Polls.Algs;

public class SingleAnswerPollAlg : IPollStrategy
{
    private IPostsDbContext _postsDbContext;
    private IEasyCachingProvider _cache;

    public SingleAnswerPollAlg(IPostsDbContext postsDbContext, IEasyCachingProvider cache)
    {
        _postsDbContext = postsDbContext;
        _cache = cache;
    }

    public string PollType => "singlePollAnswerType";

    public async Task Vote(Poll poll, OptionId optionId, long voterId)
    {
        Voter? voter = await _postsDbContext.Voters.FirstOrDefaultAsync(
            x => x.YtsooberId == voterId && x.Option.PollId == poll.Id
        );
        if (voter != null)
        {
            throw new AlreadyVotedException(optionId);
        }

        Voter voterCreated = new Voter(SnowFlakIdGenerator.NewId(), voterId, optionId);
        await _postsDbContext.Voters.AddAsync(voterCreated);
    }

    public async Task Unvote(Poll poll, OptionId optionId, long voterId)
    {
        Voter? voter = await _postsDbContext.Voters.FirstOrDefaultAsync(
            x => x.YtsooberId == voterId && x.Option.Id == optionId
        );
        if (voter == null)
            throw new VoterNotVotedException(optionId);
        _postsDbContext.Voters.Remove(voter);
        await _postsDbContext.SaveChangesAsync();
    }
}

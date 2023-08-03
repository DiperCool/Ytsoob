using BuildingBlocks.Core.IdsGenerator;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Exceptions.Domains;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Polls.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Polls.Algs;

public class SingleAnswerPollAlg : IPollStrategy
{
    private IPostsDbContext _postsDbContext;

    public SingleAnswerPollAlg(IPostsDbContext postsDbContext)
    {
        _postsDbContext = postsDbContext;
    }

    public string PollType => "singlePollAnswerType";

    public async Task Vote(Poll poll, OptionId optionId, long voterId)
    {
        Voter? voter = await _postsDbContext.Voters.FirstOrDefaultAsync(
            x => x.YtsooberId == voterId && x.Option.PollId == poll.Id
        );
        if (voter != null)
        {
            _postsDbContext.Voters.Remove(voter);
        }

        Voter voterCreated = new Voter(SnowFlakIdGenerator.NewId(), voterId, optionId);
        await _postsDbContext.Voters.AddAsync(voterCreated);
    }
}

using BuildingBlocks.Core.IdsGenerator;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Polls.ValueObjects;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Polls.Algs;

public class MultiplePollAnswerAlg : IPollStrategy
{
    private IPostsDbContext _postsDbContext;

    public MultiplePollAnswerAlg(IPostsDbContext postsDbContext)
    {
        _postsDbContext = postsDbContext;
    }

    public string PollType => "multiplePollAnswerType";

    public async Task Vote(Poll poll, OptionId optionId, long voterId)
    {
        Voter? voter = await _postsDbContext.Voters.FirstOrDefaultAsync(
            x => x.YtsooberId == voterId && x.Option.Id == optionId
        );
        if (voter != null)
        {
            return;
        }

        Voter voterCreated = new Voter(SnowFlakIdGenerator.NewId(), voterId, optionId);
        await _postsDbContext.Voters.AddAsync(voterCreated);
    }
}

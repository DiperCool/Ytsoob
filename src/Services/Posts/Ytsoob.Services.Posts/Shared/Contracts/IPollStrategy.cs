using Ytsoob.Services.Posts.Polls.Models;
using Ytsoob.Services.Posts.Polls.ValueObjects;

namespace Ytsoob.Services.Posts.Shared.Contracts;

public interface IPollStrategy
{
    public string PollType { get; }
    public Task Vote(Poll poll, OptionId optionId, long voterId);
    public Task Unvote(Poll poll, OptionId optionId, long voterId);
    public bool Check(string pollType)
    {
        return pollType == PollType;
    }
}

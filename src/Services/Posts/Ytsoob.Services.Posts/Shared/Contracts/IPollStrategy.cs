using Ytsoob.Services.Posts.Polls.ValueObjects;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Shared.Contracts;

public interface IPollStrategy
{
    public string PollType { get; }
    public Task Vote(Polls.Models.Poll poll, OptionId optionId, long voterId);
    public bool Check(string pollType)
    {
        return pollType == PollType;
    }
}
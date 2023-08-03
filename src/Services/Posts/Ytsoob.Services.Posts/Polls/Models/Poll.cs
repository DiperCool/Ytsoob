using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Exception.Types;
using BuildingBlocks.Core.IdsGenerator;
using SharpCompress.Common;
using Ytsoob.Services.Posts.Polls.Enums;
using Ytsoob.Services.Posts.Polls.ValueObjects;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Posts.ValueObjects;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Polls.Models;

public class Poll : Entity<PollId>
{
    private List<Option> _options = new();
    public IReadOnlyCollection<Option> Options => _options.ToList();
    public PostId PostId { get; private set; }
    public Post Post { get; private set; }
    public string PollAnswerType { get; private set; }

    // ef core
    protected Poll() { }

    protected Poll(PollId id, IEnumerable<Option> options, string pollAnswerType)
    {
        Id = id;
        _options = options.ToList();
        PollAnswerType = pollAnswerType;
    }

    public static Poll Create(PollId id, IEnumerable<string> options, string pollAnswerType)
    {
        IEnumerable<Option> optionsMap = options.Select(
            x => Option.Create(OptionId.Of(SnowFlakIdGenerator.NewId()), OptionTitle.Of(x))
        );
        Poll poll = new Poll(id, optionsMap, pollAnswerType);
        return poll;
    }

    public void Vote(Ytsoober voter, Option option)
    {
        if (_options.All(x => x.Id != option.Id))
        {
            throw new ArgumentException("Poll doesn't have this option");
        }
    }
}

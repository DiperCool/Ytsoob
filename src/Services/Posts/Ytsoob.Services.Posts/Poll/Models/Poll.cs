using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Posts.Poll.ValueObjects;
using Ytsoob.Services.Posts.Posts.Models;
using Ytsoob.Services.Posts.Posts.ValueObjects;

namespace Ytsoob.Services.Posts.Poll.Models;

public class Poll : Entity<PollId>
{
    private List<Option> _options = new();
    public IReadOnlyCollection<Option> Options => _options.ToList();
    public PostId PostId { get; private set; }
    public Post Post { get; private set; }

    // ef core
    protected Poll() { }

    protected Poll(IEnumerable<Option> options)
    {
        _options = options.ToList();
    }

    public static Poll Create(IEnumerable<string> options)
    {
        IEnumerable<Option> optionsMap = options.Select(x => Option.Create(OptionTitle.Of(x)));
        Poll poll = new Poll(optionsMap);
        return poll;
    }
}

using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Posts.Polls.ValueObjects;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Polls.Models;

public class Option : Entity<OptionId>
{
    public OptionTitle Title { get; private set; } = null!;

    public OptionCount Count { get; private set; } = null!;
    public Fiction Fiction { get; private set; } = null!;
    public PollId PollId { get; private set; }
    public Poll Poll { get; set; }

    // ef core
    protected Option() { }

    protected Option(OptionId id, OptionTitle title)
    {
        Id = id;
        Title = title;
        Count = OptionCount.Of(0);
        Fiction = Fiction.Of(0);
    }

    public static Option Create(OptionId optionId, OptionTitle title)
    {
        Option option = new Option(optionId, title);
        return option;
    }
}

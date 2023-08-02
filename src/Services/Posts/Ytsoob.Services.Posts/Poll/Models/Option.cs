using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Posts.Poll.ValueObjects;

namespace Ytsoob.Services.Posts.Poll.Models;

public class Option : Entity<OptionId>
{
    public OptionTitle Title { get; private set; } = null!;

    public OptionCount Count { get; private set; } = null!;
    public Fiction Fiction { get; private set; } = null!;

    private List<Voter> _voters = new();

    public IReadOnlyCollection<Voter> Voters => _voters.ToList();

    // ef core
    protected Option() { }

    protected Option(OptionTitle title)
    {
        Title = title;
        Count = OptionCount.Of(0);
        Fiction = Fiction.Of(0);
    }

    public static Option Create(OptionTitle title)
    {
        Option option = new Option(title);
        return option;
    }
}

using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Posts.Poll.ValueObjects;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Poll.Models;

public class Voter : Entity<long>
{
    public long YtsooberId { get; set; }
    public Ytsoober Ytsoober { get; set; } = null!;

    public OptionId OptionId { get; set; }
    public Option Option { get; set; } = null!;

    public Voter(long ytsooberId, OptionId optionId)
    {
        YtsooberId = ytsooberId;
        OptionId = optionId;
    }
}

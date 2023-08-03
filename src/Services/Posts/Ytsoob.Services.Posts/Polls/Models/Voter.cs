using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Posts.Polls.ValueObjects;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Polls.Models;

public class Voter : Entity<long>
{
    public long YtsooberId { get; set; }
    public Ytsoober Ytsoober { get; set; } = null!;

    public OptionId OptionId { get; set; }
    public Option Option { get; set; } = null!;

    public Voter(long id, long ytsooberId, OptionId optionId)
    {
        Id = id;
        YtsooberId = ytsooberId;
        OptionId = optionId;
    }
}

using BuildingBlocks.Abstractions.Domain;

namespace Ytsoob.Services.Posts.Poll.ValueObjects;

public record PollId : AggregateId
{
    protected PollId(long value) : base(value)
    {
    }

    public static PollId Of(long value) => new(value);
}

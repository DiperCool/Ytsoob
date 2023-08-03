using BuildingBlocks.Abstractions.Domain;

namespace Ytsoob.Services.Posts.Polls.ValueObjects;

public record OptionId : AggregateId
{
    protected OptionId(long value)
        : base(value) { }

    public static OptionId Of(long value) => new(value);
}

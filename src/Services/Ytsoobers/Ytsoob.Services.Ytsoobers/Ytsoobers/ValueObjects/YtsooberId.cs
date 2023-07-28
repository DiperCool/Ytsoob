using BuildingBlocks.Abstractions.Domain;

namespace Ytsoob.Services.Ytsoobers.Ytsoobers.ValueObjects;

public record YtsooberId : AggregateId
{
    protected YtsooberId(long value) : base(value)
    {
    }

    public static YtsooberId Of(long value) => new(value);
}

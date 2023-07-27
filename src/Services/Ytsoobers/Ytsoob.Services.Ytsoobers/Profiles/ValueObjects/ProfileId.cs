using BuildingBlocks.Abstractions.Domain;

namespace Ytsoob.Services.Ytsoobers.Profiles.ValueObjects;

public record ProfileId : AggregateId
{
    protected ProfileId(long value) : base(value)
    {
    }
    public static ProfileId Of(long value) => new(value);
}

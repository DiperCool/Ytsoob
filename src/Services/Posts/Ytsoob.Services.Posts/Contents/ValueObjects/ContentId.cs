using BuildingBlocks.Abstractions.Domain;

namespace Ytsoob.Services.Posts.Contents.ValueObjects;

public record ContentId : AggregateId
{
    protected ContentId(long value) : base(value)
    {
    }

    public static ContentId Of(long value) => new(value);
}

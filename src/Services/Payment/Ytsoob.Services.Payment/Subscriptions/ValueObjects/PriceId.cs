using BuildingBlocks.Abstractions.Domain;

namespace Ytsoob.Services.Payment.Subscriptions.ValueObjects;

public record PriceId : AggregateId
{
    protected PriceId(long value)
        : base(value) { }

    public static PriceId Of(long value) => new(value);
}

using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Payment.Subscriptions.ValueObjects;

namespace Ytsoob.Services.Payment.Subscriptions.Models;

public class PriceProduct : Entity<PriceId>
{
    protected PriceProduct() { }

    public PriceProduct(PriceId priceId, decimal price, string pricePaymentId, int recurringDays) =>
        (Id, Price, PricePaymentId, RecurringDays) = (priceId, price, pricePaymentId, recurringDays);

    public decimal Price { get; private set; }
    public string PricePaymentId { get; private set; } = default!;
    public int RecurringDays { get; private set; }
    public long SubscriptionId { get; private set; }
    public Subscription Subscription { get; private set; } = default!;
}

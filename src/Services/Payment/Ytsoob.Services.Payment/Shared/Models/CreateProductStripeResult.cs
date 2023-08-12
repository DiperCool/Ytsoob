namespace Ytsoob.Services.Payment.Shared.Models;

public record CreateProductStripeResult(string Id, IEnumerable<PriceProductResult> Prices);

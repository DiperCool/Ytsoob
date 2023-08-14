using Stripe;
using Ytsoob.Services.Payment.Shared.Contracts;
using Ytsoob.Services.Payment.Shared.Models;
using Ytsoob.Services.Payment.Subscriptions.Enums;
using Ytsoob.Services.Payment.Ytsoobers.Features.Models;
using Subscription = Ytsoob.Services.Payment.Subscriptions.Models.Subscription;

namespace Ytsoob.Services.Payment.Shared.Services;

public class PaymentService : IPaymentService
{
    public static ReadOnlyDictionary<Recurring, int> Reccurings =
        new(
            new Dictionary<Recurring, int>()
            {
                { Recurring.OneMonth, 1 },
                { Recurring.TwelveMonth, 12 },
                { Recurring.SixMonth, 6 },
                { Recurring.ThreeMonth, 3 },
            }
        );

    private record RecurringResult(decimal Price, int Month, Recurring Recurring);

    public async Task<string> CreateStripeUser(Ytsoober ytsoober)
    {
        CustomerCreateOptions customerOptions = new CustomerCreateOptions
        {
            Name = ytsoober.Username,
            Email = ytsoober.Email,
        };
        CustomerService service = new CustomerService();
        Customer customer = await service.CreateAsync(customerOptions);
        return customer.Id;
    }

    public async Task RemoveSubProduct(string productId)
    {
        var service = new ProductService();
        await service.DeleteAsync(productId);
    }

    public async Task<CreateProductStripeResult> CreateSubProduct(Subscription subscription)
    {
        var options = new ProductCreateOptions
        {
            Name = subscription.Title,
            Description = subscription.Description,
            Metadata = new Dictionary<string, string>() { { "SubId", subscription.Id.ToString() } }
        };
        if (subscription.Photo != null)
        {
            options.Images = new List<string>() { subscription.Photo };
        }

        var service = new ProductService();
        Product prodcuct = await service.CreateAsync(options);
        var prices = CalculateRecurringResults(subscription.Price);

        IEnumerable<PriceProductResult> pricesResult = await Task.WhenAll(
            prices.Select(x => CreateRecurringPrice(prodcuct.Id, x.Price, x.Month))
        );

        return new CreateProductStripeResult(prodcuct.Id, pricesResult);
    }

    public async Task UpdateSubProduct(Subscription subscription)
    {
        ProductService service = new ProductService();
        ProductUpdateOptions options = new ProductUpdateOptions()
        {
            Description = subscription.Description,
            Name = subscription.Title,
        };
        if (subscription.Photo != null)
        {
            options.Images = new List<string>() { subscription.Photo };
        }

        await service.UpdateAsync(subscription.ProductId, options);
    }

    private async Task<PriceProductResult> CreateRecurringPrice(string productId, decimal price, int months)
    {
        PriceService priceService = new PriceService();
        PriceCreateOptions priceCreateOptions = new PriceCreateOptions()
        {
            Product = productId,
            UnitAmountDecimal = price * 100,
            Currency = "USD",
            Recurring = new PriceRecurringOptions() { IntervalCount = months, Interval = "month" }
        };
        Price priceObj = await priceService.CreateAsync(priceCreateOptions);
        return new PriceProductResult(price, priceObj.Id, months);
    }

    private IEnumerable<RecurringResult> CalculateRecurringResults(decimal oneMonthPrice)
    {
        List<Recurring> recurrings = Reccurings.Keys.ToList();
        return recurrings.Select(x => new RecurringResult(oneMonthPrice * Reccurings[x], Reccurings[x], x));
    }
}

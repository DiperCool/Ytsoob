using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.IdsGenerator;
using Ytsoob.Services.Payment.Shared.Models;
using Ytsoob.Services.Payment.Subscriptions.ValueObjects;
using Ytsoob.Services.Payment.Ytsoobers.Features.Models;

namespace Ytsoob.Services.Payment.Subscriptions.Models;

public class Subscription : Entity<long>
{
    protected Subscription() { }

    public Subscription(long id, string title, string description, string? photo, decimal price, long ytsooberId)
    {
        Id = id;
        Title = title;
        Description = description;
        Photo = photo;
        Price = price;
        YtsooberId = ytsooberId;
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public string? Photo { get; private set; }
    public decimal Price { get; private set; }
    public Ytsoober Ytsoober { get; private set; } = default!;
    public long YtsooberId { get; private set; }
    public ICollection<PriceProduct> Prices { get; private set; }
    public string ProductId { get; private set; }

    public void SetStripeProduct(CreateProductStripeResult product)
    {
        Prices = product.Prices
            .Select(x => new PriceProduct(PriceId.Of(SnowFlakIdGenerator.NewId()), x.Price, x.PriceId, x.RecurringDays))
            .ToList();
        ProductId = product.Id;
    }

    public void Update(string title, string description, string? photo)
    {
        (Title, Description, Photo) = (title, description, photo);
    }
}

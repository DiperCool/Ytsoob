using BuildingBlocks.Core.Domain;
using Ytsoob.Services.Subscriptions.Subscriptions.Features.CreatingSubscription.v1.CreateSubscription;
using Ytsoob.Services.Subscriptions.Subscriptions.Features.RemovingSubscription.v1.RemoveSubscription;
using Ytsoob.Services.Subscriptions.Subscriptions.Features.UpdatingSubscription.v1.UpdateSubscription;
using Ytsoob.Services.Subscriptions.Subscriptions.ValueObjects;
using Ytsoob.Services.Subscriptions.Ytsoobers.Models;

namespace Ytsoob.Services.Subscriptions.Subscriptions.Models;

public class Subscription : Aggregate<SubscriptionId>
{
    public Title Title { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public string? Photo { get; private set; }
    public Price Price { get; private set; }
    public Ytsoober Ytsoober { get; set; } = default!;
    public long YtsooberId { get; set; }

    // ef core
    protected Subscription() { }

    protected Subscription(SubscriptionId id, Title title, Description description, Price price, long ytsooberId) =>
        (Id, Title, Description, Price, YtsooberId) = (id, title, description, price, ytsooberId);

    public static Subscription Create(
        SubscriptionId id,
        Title title,
        Description description,
        Price price,
        long ytsooberId
    )
    {
        Subscription subscription = new Subscription(id, title, description, price, ytsooberId);
        subscription.AddDomainEvents(
            new SubscriptionCreatedDomainEvent(id, title, description, null, price, ytsooberId)
        );
        return subscription;
    }

    public void Update(Title title, Description description, string? photo)
    {
        (Title, Description, Photo) = (title, description, photo);
        AddDomainEvents(new SubscriptionUpdatedDomainEvent(Id, Title, Description, Photo));
    }

    public void Remove()
    {
        AddDomainEvents(new SubscriptionRemovedDomainEvent(Id));
    }
}

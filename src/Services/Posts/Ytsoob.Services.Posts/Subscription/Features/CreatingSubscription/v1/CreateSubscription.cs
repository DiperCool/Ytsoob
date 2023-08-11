using BuildingBlocks.Abstractions.CQRS.Commands;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Subscription.Features.CreatingSubscription.v1;

public record CreateSubscription(
    long Id,
    string Title,
    string Description,
    string? Photo,
    decimal Price,
    long YtsooberId
) : ICommand;

public class CreateSubscriptionHandler : ICommandHandler<CreateSubscription>
{
    private IPostsDbContext _postsDbContext;

    public CreateSubscriptionHandler(IPostsDbContext postsDbContext)
    {
        _postsDbContext = postsDbContext;
    }

    public async Task<Unit> Handle(CreateSubscription request, CancellationToken cancellationToken)
    {
        Models.Subscription subscription = new Models.Subscription(
            request.Id,
            request.Title,
            request.Description,
            request.Photo,
            request.Price,
            request.YtsooberId
        );
        await _postsDbContext.Subscriptions.AddAsync(subscription, cancellationToken);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
    }
}

using BuildingBlocks.Abstractions.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Subscriptions.Features.RemovingSubscription.v1.RemoveSubscription;

public record RemoveSubscription(long Id) : ICommand;

public class RemoveSubscriptionHandler : ICommandHandler<RemoveSubscription>
{
    private IPostsDbContext _postsDbContext;

    public RemoveSubscriptionHandler(IPostsDbContext postsDbContext)
    {
        _postsDbContext = postsDbContext;
    }

    public async Task<Unit> Handle(RemoveSubscription request, CancellationToken cancellationToken)
    {
        Models.Subscription? subscription = await _postsDbContext.Subscriptions.FirstOrDefaultAsync(
            x => x.Id == request.Id,
            cancellationToken: cancellationToken
        );
        if (subscription == null)
            return Unit.Value;
        _postsDbContext.Subscriptions.Remove(subscription);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

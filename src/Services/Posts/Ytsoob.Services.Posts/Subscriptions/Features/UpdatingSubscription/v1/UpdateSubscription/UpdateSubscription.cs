using BuildingBlocks.Abstractions.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Posts.Shared.Contracts;

namespace Ytsoob.Services.Posts.Subscriptions.Features.UpdatingSubscription.v1.UpdateSubscription;

public record UpdateSubscription(long Id, string Title, string Description, string? Photo) : ICommand;

public class UpdatedSubscriptionHandler : ICommandHandler<UpdateSubscription>
{
    private IPostsDbContext _postsDbContext;

    public UpdatedSubscriptionHandler(IPostsDbContext postsDbContext)
    {
        _postsDbContext = postsDbContext;
    }

    public async Task<Unit> Handle(UpdateSubscription request, CancellationToken cancellationToken)
    {
        Models.Subscription? subscription = await _postsDbContext.Subscriptions.FirstOrDefaultAsync(
            x => x.Id == request.Id,
            cancellationToken: cancellationToken
        );
        if (subscription == null)
            return Unit.Value;
        subscription.Description = request.Description;
        subscription.Photo = request.Photo;
        subscription.Title = request.Title;
        _postsDbContext.Subscriptions.Update(subscription);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

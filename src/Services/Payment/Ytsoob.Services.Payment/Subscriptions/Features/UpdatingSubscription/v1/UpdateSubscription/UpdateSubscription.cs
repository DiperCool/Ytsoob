using BuildingBlocks.Abstractions.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Payment.Shared.Contracts;

namespace Ytsoob.Services.Payment.Subscriptions.Features.UpdatingSubscription.v1.UpdateSubscription;

public record UpdateSubscription(long Id, string Title, string Description, string? Photo, decimal Price) : ICommand;

public class UpdatedSubscriptionHandler : ICommandHandler<UpdateSubscription>
{
    private IPaymentDbContext _paymentDbContext;

    public UpdatedSubscriptionHandler(IPaymentDbContext paymentDbContext)
    {
        _paymentDbContext = paymentDbContext;
    }

    public async Task<Unit> Handle(UpdateSubscription request, CancellationToken cancellationToken)
    {
        Models.Subscription? subscription = await _paymentDbContext.Subscriptions.FirstOrDefaultAsync(
            x => x.Id == request.Id,
            cancellationToken: cancellationToken
        );
        if (subscription == null)
            return Unit.Value;
        subscription.Update(request.Title, request.Description, request.Photo, request.Price);
        _paymentDbContext.Subscriptions.Update(subscription);
        await _paymentDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

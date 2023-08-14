using BuildingBlocks.Abstractions.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Payment.Shared.Contracts;

namespace Ytsoob.Services.Payment.Subscriptions.Features.RemovingSubscription.v1.RemoveSubscription;

public record RemoveSubscription(long Id) : ICommand;

public class RemoveSubscriptionHandler : ICommandHandler<RemoveSubscription>
{
    private IPaymentDbContext _paymentDbContext;
    private IPaymentService _paymentService;

    public RemoveSubscriptionHandler(IPaymentDbContext postsDbContext, IPaymentService paymentService)
    {
        _paymentDbContext = postsDbContext;
        _paymentService = paymentService;
    }

    public async Task<Unit> Handle(RemoveSubscription request, CancellationToken cancellationToken)
    {
        Models.Subscription? subscription = await _paymentDbContext.Subscriptions.FirstOrDefaultAsync(
            x => x.Id == request.Id,
            cancellationToken: cancellationToken
        );
        if (subscription == null)
            return Unit.Value;
        _paymentDbContext.Subscriptions.Remove(subscription);
        await _paymentService.RemoveSubProduct(subscription.ProductId);
        await _paymentDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

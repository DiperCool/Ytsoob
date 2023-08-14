using BuildingBlocks.Abstractions.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Payment.Shared.Contracts;

namespace Ytsoob.Services.Payment.Subscriptions.Features.UpdatingSubscription.v1.UpdateSubscription;

public record UpdateSubscription(long Id, string Title, string Description, string? Photo) : ICommand;

public class UpdatedSubscriptionHandler : ICommandHandler<UpdateSubscription>
{
    private IPaymentDbContext _paymentDbContext;
    private IPaymentService _paymentService;

    public UpdatedSubscriptionHandler(IPaymentDbContext paymentDbContext, IPaymentService paymentService)
    {
        _paymentDbContext = paymentDbContext;
        _paymentService = paymentService;
    }

    public async Task<Unit> Handle(UpdateSubscription request, CancellationToken cancellationToken)
    {
        Models.Subscription? subscription = await _paymentDbContext.Subscriptions.FirstOrDefaultAsync(
            x => x.Id == request.Id,
            cancellationToken: cancellationToken
        );
        if (subscription == null)
            return Unit.Value;
        subscription.Update(request.Title, request.Description, request.Photo);
        _paymentDbContext.Subscriptions.Update(subscription);
        await _paymentService.UpdateSubProduct(subscription);
        await _paymentDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

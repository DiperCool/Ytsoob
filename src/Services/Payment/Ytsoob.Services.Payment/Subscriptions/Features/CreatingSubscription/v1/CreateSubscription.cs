using BuildingBlocks.Abstractions.CQRS.Commands;
using Ytsoob.Services.Payment.Shared.Contracts;
using Ytsoob.Services.Payment.Shared.Models;
using Ytsoob.Services.Payment.Subscriptions.Models;

namespace Ytsoob.Services.Payment.Subscriptions.Features.CreatingSubscription.v1;

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
    private IPaymentDbContext _paymentDbContext;
    private IPaymentService _paymentService;

    public CreateSubscriptionHandler(IPaymentDbContext paymentDbContext, IPaymentService paymentService)
    {
        _paymentDbContext = paymentDbContext;
        _paymentService = paymentService;
    }

    public async Task<Unit> Handle(CreateSubscription request, CancellationToken cancellationToken)
    {
        Subscription subscription = new Subscription(
            request.Id,
            request.Title,
            request.Description,
            request.Photo,
            request.Price,
            request.YtsooberId
        );

        CreateProductStripeResult product = await _paymentService.CreateSubProduct(subscription);
        subscription.SetStripeProduct(product);
        await _paymentDbContext.Subscriptions.AddAsync(subscription, cancellationToken);
        await _paymentDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using Ytsoob.Services.Payment.Shared.Contracts;
using Ytsoob.Services.Payment.Ytsoobers.Features.Models;

namespace Ytsoob.Services.Posts.Users.Features.RegisteringUser.v1.CreatingUser;

public record YtsooberCreated(Ytsoober Ytsoober) : DomainEvent;

public record CreateYtsoober(long Id, Guid IdentityId, string? Username, string Email, string Avatar) : ITxCommand;

internal class CreateYtsooberHandler : ICommandHandler<CreateYtsoober>
{
    private IPaymentDbContext _paymentDbContext;
    private ILogger<CreateYtsooberHandler> _logger;
    private IDomainEventPublisher _domainEventPublisher;
    private IPaymentService _paymentService;

    public CreateYtsooberHandler(
        IPaymentDbContext paymentDbContext,
        ILogger<CreateYtsooberHandler> logger,
        IDomainEventPublisher domainEventPublisher,
        IPaymentService paymentService
    )
    {
        _paymentDbContext = paymentDbContext;
        _logger = logger;
        _domainEventPublisher = domainEventPublisher;
        _paymentService = paymentService;
    }

    public async Task<Unit> Handle(CreateYtsoober request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating ytsoober");

        Ytsoober ytsoober = new Ytsoober()
        {
            Id = request.Id,
            Email = request.Email,
            Username = request.Username,
            IdentityId = request.IdentityId,
            Avatar = request.Avatar
        };
        string stripeId = await _paymentService.CreateStripeUser(ytsoober);
        ytsoober.StripeId = stripeId;
        await _paymentDbContext.Ytsoobers.AddAsync(ytsoober, cancellationToken);
        await _paymentDbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Ytsoober created with ID = {ID}", ytsoober.Id);

        await _domainEventPublisher.PublishAsync(new YtsooberCreated(ytsoober), cancellationToken);
        return Unit.Value;
    }
}

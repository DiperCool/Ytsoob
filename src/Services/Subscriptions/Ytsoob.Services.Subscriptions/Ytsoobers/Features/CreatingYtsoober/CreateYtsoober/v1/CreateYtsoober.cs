using BuildingBlocks.Abstractions.CQRS.Commands;
using Ytsoob.Services.Subscriptions.Shared.Contracts;
using Ytsoob.Services.Subscriptions.Ytsoobers.Models;

namespace Ytsoob.Services.Subscriptions.Ytsoobers.Features.RegisteringUser.v1.CreatingUser;

public record CreateYtsoober(long Id, Guid IdentityId, string? Username, string Email, string Avatar) : ITxCommand;

internal class CreateYtsooberHandler : ICommandHandler<CreateYtsoober>
{
    private ILogger<CreateYtsooberHandler> _logger;
    private ISubscriptionsDbContext _subscriptionsDbContext;

    public CreateYtsooberHandler(ILogger<CreateYtsooberHandler> logger, ISubscriptionsDbContext subscriptionsDbContext)
    {
        _logger = logger;
        _subscriptionsDbContext = subscriptionsDbContext;
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
        await _subscriptionsDbContext.Ytsoobers.AddAsync(ytsoober, cancellationToken);
        await _subscriptionsDbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Ytsoober created with ID = {ID}", ytsoober.Id);
        return Unit.Value;
    }
}

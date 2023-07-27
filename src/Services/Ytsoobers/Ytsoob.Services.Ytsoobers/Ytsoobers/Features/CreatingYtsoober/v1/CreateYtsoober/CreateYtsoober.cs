using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Core.Domain.ValueObjects;
using BuildingBlocks.Core.IdsGenerator;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Services.Ytsoobers.Shared.Contracts;
using Ytsoob.Services.Ytsoobers.Ytsoobers.Models;
using Ytsoob.Services.Ytsoobers.Ytsoobers.ValueObjects;

namespace Ytsoob.Services.Ytsoobers.Ytsoobers.Features.CreatingUser.v1.CreateUser;

public record CreateYtsoober(long YtsooberId, Guid IdentityId, string Username, string Email) : ITxCreateCommand;

public class CreateYtsooberHandler : ICommandHandler<CreateYtsoober>
{
    private IYtsoobersDbContext _ytsoobersDbContext;
    public CreateYtsooberHandler(IYtsoobersDbContext ytsoobersDbContext)
    {
        _ytsoobersDbContext = ytsoobersDbContext;
    }

    public async Task<Unit> Handle(CreateYtsoober request, CancellationToken cancellationToken)
    {
        if (await _ytsoobersDbContext.Ytsoobers.AnyAsync(x => x.Id == request.YtsooberId, cancellationToken: cancellationToken))
            return Unit.Value;
        Ytsoober ytsoober = Ytsoober.Create(YtsooberId.Of(request.YtsooberId), Username.Of(request.Username), Email.Of(request.Email), request.IdentityId);
        _ytsoobersDbContext.Ytsoobers.Add(ytsoober);
        await _ytsoobersDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

using BuildingBlocks.Abstractions.CQRS.Commands;
using Ytsoob.Services.Posts.Shared.Contracts;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Users.Features.RegisteringUser.v1.CreatingUser;

public record CreateYtsoober(long Id,
                             Guid IdentityId,
                             string? Username,
                             string Email) : ITxCommand;

internal class CreateYtsooberHandler : ICommandHandler<CreateYtsoober>
{
    private IPostsDbContext _postsDbContext;
    private ILogger<CreateYtsooberHandler> _logger;
    public CreateYtsooberHandler(IPostsDbContext postsDbContext, ILogger<CreateYtsooberHandler> logger)
    {
        _postsDbContext = postsDbContext;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateYtsoober request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating ytsoober");
        Ytsoober ytsoober = new Ytsoober() { Id = request.Id, Email = request.Email, Username = request.Username, IdentityId = request.IdentityId };
        await _postsDbContext.Ytsoobers.AddAsync(ytsoober, cancellationToken);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Ytsoober created with ID = {ID}", ytsoober.Id);
        return Unit.Value;
    }
}



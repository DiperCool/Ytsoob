using BuildingBlocks.Abstractions.CQRS.Commands;
using Ytsoob.Services.Posts.Shared.Contracts;
using Ytsoob.Services.Posts.Users.Features.Models;

namespace Ytsoob.Services.Posts.Users.Features.RegisteringUser.v1.CreatingUser;

public record CreateUser(Guid UserId, string Email) : ITxCommand;

internal class CreateUserHandler : ICommandHandler<CreateUser>
{
    private IPostsDbContext _postsDbContext;
    private ILogger<CreateUserHandler> _logger;
    public CreateUserHandler(IPostsDbContext postsDbContext, ILogger<CreateUserHandler> logger)
    {
        _postsDbContext = postsDbContext;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating user");
        User user = new User() { Id = request.UserId, Email = request.Email, };
        await _postsDbContext.Users.AddAsync(user, cancellationToken);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("User created with ID = {ID}", user.Id);
        return Unit.Value;
    }
}



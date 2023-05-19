using BuildingBlocks.Abstractions.CQRS.Commands;
using MassTransit;
using Ytsoob.Services.Posts.Users.Features.RegisteringUser.v1.CreatingUser;
using Ytsoob.Services.Shared.Identity.Users.Events.v1.Integration;

namespace Ytsoob.Services.Posts.Users.Features.RegisteringUser.v1.Events.Integration.External;

public class UserRegisteredConsumer : IConsumer<UserRegisteredV1>
{
    private ICommandProcessor _commandProcessor;

    public UserRegisteredConsumer(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    public async Task Consume(ConsumeContext<UserRegisteredV1> context)
    {
        var userRegistered = context.Message;
        await _commandProcessor.SendAsync(new CreateUser(userRegistered.IdentityId, userRegistered.Email));
    }
}

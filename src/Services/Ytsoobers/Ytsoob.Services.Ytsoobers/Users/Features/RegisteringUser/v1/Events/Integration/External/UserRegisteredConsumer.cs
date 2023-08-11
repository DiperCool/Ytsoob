using BuildingBlocks.Abstractions.CQRS.Commands;
using MassTransit;
using Ytsoob.Services.Shared.Identity.Users.Events.v1.Integration;
using Ytsoob.Services.Ytsoobers.Ytsoobers.Features.CreatingUser.v1.CreateUser;

namespace Ytsoob.Services.Ytsoobers.Users.Features.RegisteringUser.v1.Events.Integration.External;

public class UserRegisteredConsumer : IConsumer<UserRegisteredV1>
{
    private readonly ICommandProcessor _commandProcessor;

    public UserRegisteredConsumer(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    public async Task Consume(ConsumeContext<UserRegisteredV1> context)
    {
        var userRegistered = context.Message;
        await _commandProcessor.SendAsync(
            new CreateYtsoober(
                userRegistered.YtsooberId,
                userRegistered.IdentityId,
                userRegistered.Username,
                userRegistered.Email
            )
        );
    }
}

using BuildingBlocks.Abstractions.CQRS.Commands;
using MassTransit;
using Ytsoob.Services.Shared.Ytsoobers.Ytsoobers.Events.v1.Integration;
using Ytsoob.Services.Subscriptions.Ytsoobers.Features.RegisteringUser.v1.CreatingUser;

namespace Ytsoob.Services.Subscriptions.Ytsoobers.Features.RegisteringUser.v1.Events.Integration.External;

public class YtsooberCreatedConsumer : IConsumer<YtsooberCreatedV1>
{
    private ICommandProcessor _commandProcessor;

    public YtsooberCreatedConsumer(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    public async Task Consume(ConsumeContext<YtsooberCreatedV1> context)
    {
        var message = context.Message;
        await _commandProcessor.SendAsync(
            new CreateYtsoober(message.Id, message.IdentityId, message.Username, message.Email, message.Profile.Avatar)
        );
    }
}

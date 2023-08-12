using BuildingBlocks.Abstractions.CQRS.Commands;
using MassTransit;
using Ytsoob.Services.Posts.Users.Features.RegisteringUser.v1.CreatingUser;
using Ytsoob.Services.Shared.Ytsoobers.Ytsoobers.Events.v1.Integration;

namespace Ytsoob.Services.Payment.Ytsoobers.Features.CreatingYtsoober.v1.Events.Integration.External;

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

using BuildingBlocks.Abstractions.CQRS.Commands;
using MassTransit;
using Ytsoob.Services.Shared.Subscriptions.Subscriptions.Events.v1.Integration;

namespace Ytsoob.Services.Payment.Subscriptions.Features.UpdatingSubscription.v1.UpdateSubscription.Events.Integration;

public class SubscriptionUpdatedConsumer : IConsumer<SubscriptioUpdatedV1>
{
    private ICommandProcessor _commandProcessor;

    public SubscriptionUpdatedConsumer(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    public async Task Consume(ConsumeContext<SubscriptioUpdatedV1> context)
    {
        SubscriptioUpdatedV1 message = context.Message;
        await _commandProcessor.SendAsync(
            new UpdateSubscription(message.Id, message.Title, message.Description, message.Photo, message.Price)
        );
    }
}

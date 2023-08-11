using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Scheduler;
using MassTransit;
using Ytsoob.Services.Shared.Subscriptions.Subscriptions.Events.v1.Integration;

namespace Ytsoob.Services.Posts.Subscription.Features.CreatingSubscription.v1.Events.Integration;

public class SubscriptionCreatedConsumer : IConsumer<SubscriptionCreatedV1>
{
    private ICommandProcessor _commandProcessor;

    public SubscriptionCreatedConsumer(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    public async Task Consume(ConsumeContext<SubscriptionCreatedV1> context)
    {
        SubscriptionCreatedV1 message = context.Message;
        CreateSubscription createSubscription = new CreateSubscription(
            message.Id,
            message.Title,
            message.Description,
            message.Photo,
            message.Price,
            message.YtsooberId
        );
        await _commandProcessor.SendAsync(createSubscription);
    }
}

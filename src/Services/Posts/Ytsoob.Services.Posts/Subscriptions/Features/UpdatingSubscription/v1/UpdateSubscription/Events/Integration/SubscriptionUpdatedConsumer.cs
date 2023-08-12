using BuildingBlocks.Abstractions.CQRS.Commands;
using MassTransit;
using Ytsoob.Services.Shared.Subscriptions.Subscriptions.Events.v1.Integration;

namespace Ytsoob.Services.Posts.Subscriptions.Features.UpdatingSubscription.v1.UpdateSubscription.Events.Integration;

public class SubscriptionUpdatedConsumer : IConsumer<SubscriptioUpdatedV1>
{
    private ICommandProcessor _commandProcessor;

    public SubscriptionUpdatedConsumer(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    public async Task Consume(ConsumeContext<SubscriptioUpdatedV1> context)
    {
        SubscriptioUpdatedV1 subscriptionUpdatedV1 = context.Message;
        await _commandProcessor.SendAsync(new )
    }
}

using BuildingBlocks.Abstractions.CQRS.Commands;
using MassTransit;
using Ytsoob.Services.Shared.Subscriptions.Subscriptions.Events.v1.Integration;

namespace Ytsoob.Services.Posts.Subscriptions.Features.RemovingSubscription.v1.RemoveSubscription.Events.Integration;

public class RemovedSubscriptionConsumer : IConsumer<SubscriptionRemovedV1>
{
    private ICommandProcessor _commandProcessor;

    public RemovedSubscriptionConsumer(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    public async Task Consume(ConsumeContext<SubscriptionRemovedV1> context)
    {
        await _commandProcessor.SendAsync(new RemoveSubscription(context.Message.Id));
    }
}

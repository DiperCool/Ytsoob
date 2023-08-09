using BuildingBlocks.Abstractions.Messaging;

namespace Ytsoob.Services.Subscriptions;

public class TestIntegrationConsumer : IIntegrationEventHandler<TestIntegration>
{
    public Task Handle(TestIntegration notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("Data is: " + notification.Data);
        return Task.CompletedTask;
    }
}

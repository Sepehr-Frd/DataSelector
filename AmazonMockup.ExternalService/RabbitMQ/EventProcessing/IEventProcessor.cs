namespace AmazonMockup.ExternalService.RabbitMQ.EventProcessing;

public interface IEventProcessor
{
    Task ProcessEventAsync(string message, CancellationToken cancellationToken);
}


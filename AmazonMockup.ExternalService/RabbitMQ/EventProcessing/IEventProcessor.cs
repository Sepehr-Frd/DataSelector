namespace AmazonMockup.ExternalService.RabbitMQ.EventProcessing;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}


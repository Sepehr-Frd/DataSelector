using DataSelector.ExternalService.RabbitMQ.EventProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DataSelector.ExternalService.RabbitMQ;

public class MessageBusSubscriber : BackgroundService
{

    private readonly IConfiguration _configuration;

    private readonly IEventProcessor _eventProcessor;

    private IConnection? _connection;

    private IModel? _channel;

    private string? _queueName;
    
    private const string _triggerExchange = "trigger";

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;

        _eventProcessor = eventProcessor;

        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = _configuration.GetSection("RabbitMQ").GetValue<string>("RabbitMQHost"),
            Port = _configuration.GetSection("RabbitMQ").GetValue<int>("RabbitMQPort")
        };

        _connection = connectionFactory.CreateConnection();

        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: _triggerExchange, type: ExchangeType.Fanout);

        _queueName = _channel.QueueDeclare().QueueName;

        _channel.QueueBind(queue: _queueName, exchange: _triggerExchange, "");

        // TODO: Add logging

        Console.WriteLine("Listening on the Message Bus");

        _connection.ConnectionShutdown += RabbitMQConnectionShutdownEventHandler;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (ModuleHandle, eventArgument) =>
        {
            Console.WriteLine("Event Received!");

            var body = eventArgument.Body;

            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            await _eventProcessor.ProcessEventAsync(notificationMessage, stoppingToken);

        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    private void RabbitMQConnectionShutdownEventHandler(object? sender, ShutdownEventArgs shutdownEventArgs)
    {
        // TODO: Add logging
        Console.WriteLine($"RabbitMQ connection was shutdown by {sender}", shutdownEventArgs);
    }

    public override void Dispose()
    {
        if (_channel!.IsOpen)
        {
            _channel.Close();
            _connection!.Close();
        }

        base.Dispose();
    }
}

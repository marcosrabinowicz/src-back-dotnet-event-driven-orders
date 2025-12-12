using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Orders.Application.Abstractions.Messaging;
using Polly;
using Polly.Retry;

namespace Orders.Infrastructure.Messaging;

public sealed class AzureServiceBusMessageBus : IMessageBus
{
    private readonly ServiceBusClient _client;
    private readonly string _topicName;
    private readonly AsyncRetryPolicy _retryPolicy;


    public AzureServiceBusMessageBus(ServiceBusClient client, string topicName)
    {
        _client = client;
        _topicName = topicName;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))
            );
    }

    public async Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var sender = _client.CreateSender(_topicName);

        var body = JsonSerializer.Serialize(@event);
        var message = new ServiceBusMessage(body)
        {
            ContentType = "application/json"
        };

        message.CorrelationId = @event.CorrelationId;
        message.MessageId = @event.EventId.ToString();

        await _retryPolicy.ExecuteAsync(async () =>
        {
            await sender.SendMessageAsync(message, cancellationToken);
        });
    }
}

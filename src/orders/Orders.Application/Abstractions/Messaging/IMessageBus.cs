namespace Orders.Application.Abstractions.Messaging;

public interface IMessageBus
{
    Task PublishAsync(
        IntegrationEvent @event,
        CancellationToken cancellationToken = default
    );
}

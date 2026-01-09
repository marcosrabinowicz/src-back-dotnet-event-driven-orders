using Orders.Application.Abstractions.Messaging;

namespace Orders.Application.Interfaces;

public interface IMessageBus
{
    Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);
}

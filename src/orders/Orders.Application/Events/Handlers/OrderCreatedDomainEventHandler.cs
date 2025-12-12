using MediatR;
using Orders.Domain.Events;
using Orders.Application.Events.Integration.Mappers;
using Orders.Application.Abstractions.Messaging;

namespace Orders.Application.Events.Handlers;

public sealed class OrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
{
    private readonly IMessageBus _bus;

    public OrderCreatedDomainEventHandler(IMessageBus bus)
    {
        _bus = bus;
    }

    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: em produção, pegar o CorrelationId do contexto (HTTP, message, etc.)
        var correlationId = Guid.NewGuid().ToString();

        var integrationEvent = OrderCreatedIntegrationEventMapper.Map(notification, correlationId);

        await _bus.PublishAsync(integrationEvent, cancellationToken);
    }
}

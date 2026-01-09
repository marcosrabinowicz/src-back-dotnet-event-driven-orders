using MediatR;
using Orders.Domain.Events;
using Orders.Application.Events.Integration.Mappers;
using Orders.Application.Interfaces;

namespace Orders.Application.Events.Handlers;

public sealed class OrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
{
    private readonly IMessageBus _bus;
    private readonly ICorrelationIdProvider _correlationIdProvider;

    public OrderCreatedDomainEventHandler(IMessageBus bus, ICorrelationIdProvider correlationIdProvider)
    {
        _bus = bus;
        _correlationIdProvider = correlationIdProvider;
    }

    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // CorrelationId nasce na borda (API/consumer) e é propagado no fluxo distribuído.
        var correlationId = _correlationIdProvider.GetCorrelationId();

        if (string.IsNullOrWhiteSpace(correlationId))
            throw new InvalidOperationException("CorrelationId não pode ser nulo ou vazio.");

        // Tradução: Domain Event (interno) -> Integration Event (contrato público)
        var integrationEvent = OrderCreatedIntegrationEventMapper.Map(notification, correlationId);

        // Publicação via abstração (Infra implementa Azure Service Bus)
        await _bus.PublishAsync(integrationEvent, cancellationToken);
    }
}

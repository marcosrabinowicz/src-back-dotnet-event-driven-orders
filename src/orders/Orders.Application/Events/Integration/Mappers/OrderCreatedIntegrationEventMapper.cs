using Orders.Domain.Events;

namespace Orders.Application.Events.Integration.Mappers;

public static class OrderCreatedIntegrationEventMapper
{
    public static OrderCreatedIntegrationEvent Map(
        OrderCreatedDomainEvent domainEvent,
        string correlationId
    )
    {
        return new OrderCreatedIntegrationEvent(
            domainEvent.OrderId,
            domainEvent.CustomerId,
            domainEvent.TotalAmount.Amount,
            domainEvent.TotalAmount.Currency
        )
        {
            CorrelationId = correlationId
        };
    }
}

using Orders.Domain.Events;
using Orders.Application.Events.Integration;

namespace Orders.Application.Events.Integration.Mappers;

public static class OrderCreatedIntegrationEventMapper
{
    public static OrderCreatedIntegrationEvent Map(OrderCreatedDomainEvent domainEvent)
    {
        return new OrderCreatedIntegrationEvent(
            domainEvent.OrderId,
            domainEvent.CustomerId,
            domainEvent.TotalAmount.Amount,
            domainEvent.TotalAmount.Currency
        );
    }
}

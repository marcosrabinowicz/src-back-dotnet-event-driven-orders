using Orders.Application.Abstractions.Messaging;

namespace Orders.Application.Events.Integration;

public sealed record OrderCreatedIntegrationEvent(
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    string Currency
) : IntegrationEvent;

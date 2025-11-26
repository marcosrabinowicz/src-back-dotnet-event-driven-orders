using Orders.Domain.Abstractions;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Events;

public sealed record OrderCreatedDomainEvent(Guid OrderId, Guid CustomerId, Money TotalAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

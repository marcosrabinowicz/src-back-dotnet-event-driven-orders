using MediatR;

namespace Orders.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}

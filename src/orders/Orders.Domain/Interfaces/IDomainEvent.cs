using MediatR;

namespace Orders.Domain.Interfaces;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}

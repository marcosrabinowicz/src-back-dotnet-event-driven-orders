namespace Orders.Domain.Abstractions;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

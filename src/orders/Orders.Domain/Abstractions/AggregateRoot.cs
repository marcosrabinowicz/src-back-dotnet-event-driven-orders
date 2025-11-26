namespace Orders.Domain.Abstractions;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent @event)
    {
        _events.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _events.Clear();
    }
}

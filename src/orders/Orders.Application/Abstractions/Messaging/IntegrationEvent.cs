namespace Orders.Application.Abstractions.Messaging;

public abstract record IntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public string CorrelationId { get; init; } = default!;
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

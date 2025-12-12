namespace Orders.Application.Abstractions.Messaging;

public abstract record IntegrationEvent
{
    /// <summary>
    /// Identificador único do evento (usado para idempotência).
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Identificador de correlação da requisição (usado para rastrear o fluxo
    /// entre serviços). Idealmente nasce na borda (API / Message Broker).
    /// </summary>
    public string CorrelationId { get; init; } = default!;

    /// <summary>
    /// Momento em que o evento foi criado.
    /// </summary>
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

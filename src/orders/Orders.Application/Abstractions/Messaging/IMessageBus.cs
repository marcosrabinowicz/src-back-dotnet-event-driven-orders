namespace Orders.Application.Abstractions.Messaging;

public interface IMessageBus
{
    /// <summary>
    /// Publica uma mensagem no barramento de eventos/mensageria.
    /// A implementação concreta (infra) decide se vai para fila, tópico, etc.
    /// </summary>
    /// <typeparam name="TMessage">Tipo da mensagem de integração.</typeparam>
    /// <param name="message">Mensagem a ser publicada.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default);
}

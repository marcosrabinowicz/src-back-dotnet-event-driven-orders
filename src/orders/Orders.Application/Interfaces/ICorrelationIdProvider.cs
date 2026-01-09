namespace Orders.Application.Interfaces;

public interface ICorrelationIdProvider
{
    string GetCorrelationId();
}

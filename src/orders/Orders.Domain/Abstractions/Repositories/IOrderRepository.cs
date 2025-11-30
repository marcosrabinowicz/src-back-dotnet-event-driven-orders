using Orders.Domain.Aggregates;

namespace Orders.Domain.Abstractions.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
}

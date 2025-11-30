using MediatR;
using Orders.Domain.Aggregates;
using Orders.Domain.Abstractions.Repositories;
using Orders.Domain.Entities;
using Orders.Domain.ValueObjects;

namespace Orders.Application.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repository;

    public CreateOrderCommandHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var items = request.Items
            .Select(i => new OrderItem(
                i.ProductId,
                i.Quantity,
                new Money(i.UnitPrice) // conversão aqui!
            ))
            .ToList();

        var order = Order.Create(request.CustomerId, items);

        await _repository.AddAsync(order, cancellationToken);

        return order.Id;
    }
}

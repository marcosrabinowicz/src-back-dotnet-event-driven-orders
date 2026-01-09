using MediatR;
using Orders.Domain.Aggregates;
using Orders.Domain.Abstractions.Repositories;
using Orders.Domain.ValueObjects;
using Orders.Domain.Exceptions;

namespace Orders.Application.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repository;

    public CreateOrderCommandHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.Items is null || request.Items.Count == 0)
            throw new DomainException("Pedido deve ter pelo menos 1 item.");

        var order = Order.Create(request.CustomerId);

        foreach (var i in request.Items)
            order.AddItem(i.ProductId, i.Quantity, new Money(i.UnitPrice));

        order.Submit();

        await _repository.AddAsync(order, cancellationToken);

        return order.Id;
    }
}

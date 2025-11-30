using MediatR;

namespace Orders.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    Guid CustomerId,
    IReadOnlyCollection<CreateOrderItemDto> Items
) : IRequest<Guid>;

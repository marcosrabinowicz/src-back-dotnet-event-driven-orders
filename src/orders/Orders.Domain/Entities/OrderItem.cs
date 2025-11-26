using Orders.Domain.Exceptions;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Entities;

public sealed class OrderItem
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = default!;
    public Money Total => new Money(UnitPrice.Amount * Quantity, UnitPrice.Currency);

    private OrderItem() { } // EF Core

    public OrderItem(Guid productId, int quantity, Money unitPrice)
    {
        if (productId == Guid.Empty)
            throw new DomainException("ProductId inválido.");
        if (quantity <= 0)
            throw new DomainException("A quantidade deve ser maior que zero.");
        if (unitPrice is null)
            throw new DomainException("Preço unitário não pode ser nulo.");

        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("A quantidade deve ser maior que zero.");

        Quantity = quantity;
    }

    public void UpdateUnitPrice(Money price)
    {
        if (price is null)
            throw new DomainException("Preço unitário não pode ser nulo.");

        UnitPrice = price;
    }
}

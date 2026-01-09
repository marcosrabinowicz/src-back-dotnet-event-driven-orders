using Orders.Domain.Exceptions;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Entities;

public sealed class OrderItem
{
    public Guid Id { get; private set; }
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
            throw new DomainException("Quantidade inválida.");
        if (unitPrice is null)
            throw new DomainException("UnitPrice inválido.");

        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantidade inválida.");

        Quantity += quantity;
    }

    public void DecreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantidade inválida.");

        if (Quantity - quantity <= 0)
            throw new DomainException("Quantidade resultante inválida.");

        Quantity -= quantity;
    }
}

using Orders.Domain.Abstractions;
using Orders.Domain.Entities;
using Orders.Domain.Events;
using Orders.Domain.Exceptions;
using Orders.Domain.ValueObjects;

namespace Orders.Domain.Aggregates;

public sealed class Order : AggregateRoot
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money Total => _items
        .Select(i => i.Total)
        .Aggregate(new Money(0, "BRL"), (acc, money) => acc + money);

    private Order() { } // EF Core

    private Order(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
    }

    public static Order Create(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new DomainException("CustomerId inválido.");

        return new Order(customerId);
    }

    public void AddItem(Guid productId, int quantity, Money unitPrice)
    {
        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
            return;
        }

        _items.Add(new OrderItem(productId, quantity, unitPrice));
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId);

        if (item == null)
            throw new DomainException("Item inexistente.");

        _items.Remove(item);
    }

    public void Submit()
    {
        EnsureInvariants();

        AddDomainEvent(new OrderCreatedDomainEvent(Id, CustomerId, Total));
    }

    private void EnsureInvariants()
    {
        if (_items.Count == 0)
            throw new DomainException("Pedido deve ter pelo menos 1 item.");

        if (Total.Amount < 0)
            throw new DomainException("Total inválido.");
    }
}

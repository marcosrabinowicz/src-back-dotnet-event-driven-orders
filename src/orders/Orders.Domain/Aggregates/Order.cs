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
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money Total => _items
        .Select(i => i.Total)
        .Aggregate(new Money(0, "BRL"), (acc, money) => acc + money);

    private Order() { } // EF Core

    private Order(Guid customerId, IEnumerable<OrderItem> items)
    {
        if (customerId == Guid.Empty)
            throw new DomainException("CustomerId inválido.");

        if (items is null)
            throw new DomainException("A lista de itens não pode ser nula.");

        Id = Guid.NewGuid();
        CustomerId = customerId;

        _items.AddRange(items);

        EnsureInvariants();

        AddDomainEvent(new OrderCreatedDomainEvent(Id, CustomerId, Total));
    }

    public static Order Create(Guid customerId, IEnumerable<OrderItem> items)
    {
        return new Order(customerId, items);
    }

    // ----------------------------------------------------------------------
    // 🟦 MÉTODOS REAIS DE DOMÍNIO
    // ----------------------------------------------------------------------

    public void AddItem(Guid productId, int quantity, Money unitPrice)
    {
        var item = new OrderItem(productId, quantity, unitPrice);
        _items.Add(item);

        EnsureInvariants();
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId);

        if (item == null)
            throw new DomainException("Item inexistente.");

        _items.Remove(item);

        EnsureInvariants();
    }

    public void ChangeItemQuantity(Guid productId, int newQuantity)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
            throw new DomainException("Item inexistente.");

        item.UpdateQuantity(newQuantity);

        EnsureInvariants();
    }

    public void ChangeItemPrice(Guid productId, Money newUnitPrice)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
            throw new DomainException("Item inexistente.");

        item.UpdateUnitPrice(newUnitPrice);

        EnsureInvariants();
    }

    public void RecalculateItem(Guid productId, int newQuantity, Money newPrice)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);

        if (item is null)
            throw new DomainException("Item inexistente.");

        item.UpdateQuantity(newQuantity);
        item.UpdateUnitPrice(newPrice);

        EnsureInvariants();
    }

    // ----------------------------------------------------------------------
    // 🔒 INVARIANTES
    // ----------------------------------------------------------------------

    private void EnsureInvariants()
    {
        if (_items.Count == 0)
            throw new DomainException("Pedido deve ter pelo menos 1 item.");

        if (Total.Amount < 0)
            throw new DomainException("Total inválido.");
    }
}

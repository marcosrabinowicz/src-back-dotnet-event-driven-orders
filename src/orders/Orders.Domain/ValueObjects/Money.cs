using Orders.Domain.Exceptions;

namespace Orders.Domain.ValueObjects;

public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new DomainException("Valor negativo não permitido.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Moeda inválida.");

        Amount = amount;
        Currency = currency.ToUpper();
    }

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new DomainException("Moedas diferentes.");

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public bool Equals(Money? other)
        => other != null && Amount == other.Amount && Currency == other.Currency;

    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
}

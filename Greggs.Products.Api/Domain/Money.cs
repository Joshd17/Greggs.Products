namespace Greggs.Products.Api.Domain;

public class Money
{
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }

    public Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public Money Add(Money other)
    {
        if(IsSameCurrency(other)) 
            throw new DomainException($"Cannot add mismatching currencies {other.Currency} and {Currency} ");

        return new Money(Amount + other.Amount, Currency);
    }

    public bool IsSameCurrency(Money other)
    {
        return Currency == other.Currency;
    }

    public static Money operator +(Money a, Money b)
    {
        return a.Add(b);
    }
}

namespace Greggs.Products.Api.Domain;

public class DomainProduct
{
    public string Name { get; private set; }
    public Money Value { get; private set; }

    public DomainProduct(string name, Money value)
    {
        if (string.IsNullOrEmpty(name) || value == null)
            throw new DomainException("Name or value cannot be null");
        Name = name;
        Value = value;
    }



    public void SetValue(Currency currency, decimal exchangeRate)
    {
        Value = new Money(Value.Amount * exchangeRate, currency);
    }
}

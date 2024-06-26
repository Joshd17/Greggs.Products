namespace Greggs.Products.Api.Domain.Services;

public interface IExchangeRateService
{
    public decimal GetExchangeRate(Currency baseCurrency, Currency targetCurrency);
}

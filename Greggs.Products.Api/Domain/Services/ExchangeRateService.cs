using System;

namespace Greggs.Products.Api.Domain.Services;

public class ExchangeRateService : IExchangeRateService
{
    public decimal GetExchangeRate(Currency baseCurrency, Currency targetCurrency)//Currently untested as a fake class(ideally this would be via a store or API eg https://www.exchangerate-api.com/
    {
        if (baseCurrency == Currency.Pounds && targetCurrency == Currency.Euros)
            return 1.13M;

        throw new NotImplementedException(
            $"Fake currency provider does not have implementation for {baseCurrency} and {targetCurrency}");
    }
}

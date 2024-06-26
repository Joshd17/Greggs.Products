using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Domain;
using Greggs.Products.Api.Domain.Services;
using Greggs.Products.Api.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Greggs.Products.Api.Features.Queries;

public class GetLatestProductQueryHandler : IRequestHandler<GetLatestProductsQuery, QueryResult<IEnumerable<GetLatestProductQueryModel>>>
{
    private const int MaxPageSize = 1000;
    private readonly IDataAccess<Product> _dataAccess;
    private readonly IExchangeRateService _exchangeRateService;

    public GetLatestProductQueryHandler(IDataAccess<Product> dataAccess, IExchangeRateService exchangeRateService)
    {
        _dataAccess = dataAccess;
        _exchangeRateService = exchangeRateService;
    }

    public async Task<QueryResult<IEnumerable<GetLatestProductQueryModel>>> Handle(GetLatestProductsQuery request, CancellationToken cancellationToken)
    {
        if (request.PageSize > 10000)//we could query products here to check - just would always result in extra calls unless caching etc
            return QueryResult<IEnumerable<GetLatestProductQueryModel>>.Fail(new string[]{$"Unable to query products larger than {MaxPageSize}"});

        if (request.PageSize == null)
            return QueryResult<IEnumerable<GetLatestProductQueryModel>>.Fail(new string[] { $"Pagesize is required" });

        IEnumerable<DomainProduct> values = _dataAccess.List(request.PageStart, request.PageSize).Select(x => new DomainProduct(x.Name, new Money(x.PriceInPounds, Currency.Pounds)));

        if (request.RequestedCurrency != Currency.Pounds)
        {
            var rate = _exchangeRateService.GetExchangeRate(Currency.Pounds, request.RequestedCurrency);

            var convertedValues = new List<DomainProduct>();
            foreach (var domainProduct in values)
            {
                var product = new DomainProduct(domainProduct.Name, domainProduct.Value);
                product.SetValue(Currency.Pounds, rate);
                convertedValues.Add(product);
            }

            values = convertedValues;
        }



        return QueryResult<IEnumerable<GetLatestProductQueryModel>>.Success(values.Select(x => new GetLatestProductQueryModel(x.Value.Amount, x.Name)));
    }
}

public class GetLatestProductsQuery : IRequest<QueryResult<IEnumerable<GetLatestProductQueryModel>>>
{
    public int? PageStart { get; init; }
    public int? PageSize { get; init; }
    public Currency RequestedCurrency { get; init; }
}

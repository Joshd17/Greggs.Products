using FluentAssertions;
using Greggs.Products.Api;
using Greggs.Products.Api.Domain;
using Greggs.Products.Api.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Greggs.Products.IntegrationTests;

public class QueryHandlerIntegrationTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program>
        _factory;

    public QueryHandlerIntegrationTests(
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    // This is just a sample - this ideally would be extended to cover more scenarios
    [Fact]
    public async Task Query_ConvertingToEuros_Converts()
    {
        var serviceScopeFactory = _factory.Services.GetService<IServiceScopeFactory>();


        using IServiceScope scope = serviceScopeFactory.CreateScope();
        var mediator =
            scope.ServiceProvider.GetRequiredService<IMediator>();

        var query = await mediator.Send(new GetLatestProductsQuery
            { PageSize = 1, PageStart = 0, RequestedCurrency = Currency.Euros });

        query.Value.First().Price.Should().Be(1.13M);
    }
}
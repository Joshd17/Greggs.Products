using FakeItEasy;
using FluentAssertions;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Domain.Services;
using Greggs.Products.Api.Domain;
using Greggs.Products.Api.Features.Queries;
using Greggs.Products.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Greggs.Products.UnitTests.Queries;
public class GetLatestProductQueryHandlerTests
{
    private readonly IDataAccess<Product> _dataAccess;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly GetLatestProductQueryHandler _handler;
    public GetLatestProductQueryHandlerTests()
    {
        _dataAccess = A.Fake<IDataAccess<Product>>();
        _exchangeRateService = A.Fake<IExchangeRateService>();
        _handler = new GetLatestProductQueryHandler(_dataAccess, _exchangeRateService);
    }
    [Fact]
    public async Task Handle_ReturnsQueryResult_WhenValid()
    {
        // Arrange
        var products = new List<Product> { new Product { Name = "Test", PriceInPounds = 1.0m } };
        A.CallTo(() => _dataAccess.List(A<int?>._, A<int?>._)).Returns(products);
        var request = new GetLatestProductsQuery { PageStart = 0, PageSize = 1, RequestedCurrency = Currency.Pounds };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        // Assert
        result.Value.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
        
        var returnValue = result.Value.ToList();
        returnValue.Count.Should().Be(1);
        returnValue[0].Name.Should().Be(products[0].Name);
        returnValue[0].Price.Should().Be(products[0].PriceInPounds);
    }
    
    [Fact]
    public async Task Handle_ReturnsErrors_WhenPageSizeIsTooLarge()
    {
        // Arrange
        var request = new GetLatestProductsQuery { PageStart = 0, PageSize = 10001, RequestedCurrency = Currency.Pounds };
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var error = result.Errors.Single();
        result.Value.Should().BeNull();
        error.Should().Contain("larger");
    }

    [Fact]
    public async Task Handle_ReturnsErrors_WhenPageSizeIsNll()
    {
        // Arrange
        var request = new GetLatestProductsQuery { PageStart = 0, PageSize = null, RequestedCurrency = Currency.Pounds };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        var error = result.Errors.Single();
        result.Value.Should().BeNull();
        error.Should().Contain("required");
    }
}

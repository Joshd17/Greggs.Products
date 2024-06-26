using FakeItEasy;
using FluentAssertions;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.Features.Queries;
using Greggs.Products.Api.Features;
using Greggs.Products.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Greggs.Products.UnitTests.Controller;
public class ProductControllerTests
{
    private readonly ILogger<ProductController> _logger;
    private readonly IMediator _mediator;
    private readonly ProductController _controller;
    public ProductControllerTests()
    {
        _logger = A.Fake<ILogger<ProductController>>();
        _mediator = A.Fake<IMediator>();
        _controller = new ProductController(_logger, _mediator);
    }
    [Fact]
    public async Task Get_ReturnsOkResult_WhenNoErrors()
    {
        // Arrange
        var products = new List<GetLatestProductQueryModel> { new(1.0m, "yum yum") };
        var queryResult = QueryResult<IEnumerable<GetLatestProductQueryModel>>.Success(products);
        A.CallTo(() => _mediator.Send(A<GetLatestProductsQuery>.Ignored, A<CancellationToken>._)).Returns(queryResult);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        var returnValue = okResult.Value as List<GetLatestProductQueryModel>;
        returnValue!.Count.Should().Be(1);
        returnValue.First().Should().BeEquivalentTo(products.First());
    }

    [Fact]
    public async Task Get_ReturnsBadRequest_WhenQueryErrors()
    {
        // Arrange
        var errors = new List<string> { "Error" };
        var queryResult = QueryResult<IEnumerable<GetLatestProductQueryModel>>.Fail(errors);
        A.CallTo(() => _mediator.Send(A<GetLatestProductsQuery>._, A<CancellationToken>._)).Returns(queryResult);

        // Act
        var result = await _controller.Get();

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
        var returnValue = badRequestResult.Value as List<string>;
        returnValue!.Count.Should().Be(1);
        returnValue[0].Should().Be(errors[0]);
    }
}

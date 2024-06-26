using Greggs.Products.Api.Domain;
using Greggs.Products.Api.Features.Queries;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IMediator _mediator;

    public ProductController(ILogger<ProductController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> Get(int pageStart = 0, int pageSize = 5, Currency currency = Currency.Pounds)
    {
        var queryResult = await _mediator.Send(new GetLatestProductsQuery { PageSize = pageSize, PageStart = pageStart, RequestedCurrency = currency});

        if (queryResult.Errors.Any())
            return new BadRequestObjectResult(queryResult.Errors);

        _logger.LogInformation("Products GET request returning");

        return Ok(queryResult.Value);
    }
}
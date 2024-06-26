namespace Greggs.Products.Api.Features.Queries;

public record GetLatestProductQueryModel
{
    public decimal Price { get; }
    public string Name { get; }
    public GetLatestProductQueryModel(decimal price, string name)
    {
        Price = price;
        Name = name;
    }
}

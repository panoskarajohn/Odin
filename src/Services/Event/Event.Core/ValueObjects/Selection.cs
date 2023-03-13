using Event.Core.Exceptions;

namespace Event.Core.ValueObjects;

public record Selection
{
    public const decimal MinPrice = decimal.Zero;
    public const decimal MaxPrice = 1000M;

    public Selection(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new SelectionInvalidNameException();

        if (price is < MinPrice or > MaxPrice)
            throw new SelectionInvalidPriceException();

        Name = name;
        Price = price;
    }

    public string Name { get; }
    public decimal Price { get; }
}
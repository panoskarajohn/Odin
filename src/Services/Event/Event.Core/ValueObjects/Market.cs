using Event.Core.Exceptions;

namespace Event.Core.ValueObjects;

public record Market
{
    public Market(string name, IEnumerable<Selection> selections)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new MarketInvalidNameException();

        Name = name;
        Selections = selections ?? throw new MarketShouldContainSelections();
    }

    public string Name { get; }

    public IEnumerable<Selection> Selections { get; }
}
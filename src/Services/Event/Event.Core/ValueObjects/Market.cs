using Event.Core.Exceptions;

namespace Event.Core.ValueObjects;

public record Market
{
    public Market(string name, IEnumerable<Selection> outcomes)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new MarketInvalidNameException();

        Name = name;
        Outcomes = outcomes ?? throw new MarketShouldContainSelections();
    }

    public string Name { get; }

    public IEnumerable<Selection> Outcomes { get; }
}
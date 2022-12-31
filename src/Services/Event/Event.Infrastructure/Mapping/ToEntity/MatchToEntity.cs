using Event.Core.Models;
using Event.Core.ValueObjects;
using Event.Infrastructure.Mongo;

namespace Event.Infrastructure.Mapping.ToEntity;

public static class MatchToEntity
{
    public static Match AsEntity(this MatchDocument document)
    {
        var match = Match.Create(document.Category,
            document.StartingTime,
            document.Home,
            document.Away,
            document.Status,
            document.Id);

        if (document.Markets?.Any() ?? false)
            match.AppendMarkets(document
                .Markets.Select(m => m.AsEntity()));

        match.Version = document.Version;

        return match;
    }

    public static Market AsEntity(this MarketDocument document)
    {
        return new Market(document.Name, document.SelectionDocuments.Select(s => s.AsEntity()));
    }

    public static Selection AsEntity(this SelectionDocument document)
    {
        return new Selection(document.Name, document.Price);
    }
}
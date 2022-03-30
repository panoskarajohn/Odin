using Event.Core.Models;
using Event.Core.ValueObjects;
using Event.Infrastructure.Mongo;

namespace Event.Infrastructure.Mapping;

public static class MatchToDocument
{
    public static MatchDocument AsDocument(this Match match)
    {
        (string home, string away) = match.MatchName;

        var matchDocument = new MatchDocument(match.Id,
            match.Category,
            match.StartingTime,
            match.MatchName.Value,
            home, away,
            match.Status.Name);

        if (match.Markets.Any())
        {
            matchDocument.Markets = match.Markets.Select(m => m.AsDocument());
        }

        return matchDocument;
    }

    public static MarketDocument AsDocument(this Market market)
    {
        return new MarketDocument(market.Name,
            market.Selections.Select(s => s.AsDocument()));
    }

    public static SelectionDocument AsDocument(this Selection selection)
    {
        return new SelectionDocument(selection.Name, selection.Price);
    }

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
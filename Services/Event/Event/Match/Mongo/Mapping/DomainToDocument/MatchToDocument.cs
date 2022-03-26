using Event.Match.Mongo.Document;

namespace Event.Match.Mongo.Mapping.DomainToDocument;

public static class MatchToDocument
{
    public static MatchDocument AsDocument(this Match.Models.Match match)
    {
        (string home, string away) = match.MatchName;
        return new(match.Id, match.Category, match.StartingTime, match.MatchName.Value, home, away);
    }

    public static Models.Match AsEntity(this MatchDocument document)
    {
        var match = Models.Match.Create(document.Category, document.StartingTime, document.Home, document.Away,
            document.Id);
        return match;
    }
}
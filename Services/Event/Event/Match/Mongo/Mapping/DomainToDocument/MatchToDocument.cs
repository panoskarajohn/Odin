using Event.Match.Mongo.Document;

namespace Event.Match.Mongo.Mapping.DomainToDocument;

public static class MatchToDocument
{
    public static MatchDocument AsDocument(this Match.Models.Match match)
    {
        return new(match.Id, match.Category, match.StartingTime);
    }
}
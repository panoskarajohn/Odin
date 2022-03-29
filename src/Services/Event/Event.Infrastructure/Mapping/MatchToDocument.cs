using Event.Core.Enumerations;
using Event.Infrastructure.Mongo;
using Event.Core.Models;
namespace Event.Infrastructure.Mapping;

public static class MatchToDocument
{
    public static MatchDocument AsDocument(this Match match)
    {
        (string home, string away) = match.MatchName;
        return new(match.Id, match.Category, match.StartingTime, match.MatchName.Value, home, away, match.Status.Name);
    }

    public static Match AsEntity(this MatchDocument document)
    {
        var match = Match.Create(document.Category, 
                                document.StartingTime, 
                                  document.Home, 
                                  document.Away,
                                document.Status,
                                  document.Id);
        return match;
    }
}
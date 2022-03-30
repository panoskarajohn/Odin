using Shared.Mongo;

namespace Event.Infrastructure.Mongo;

public class MatchDocument : IIdentifiable<long>
{
    public MatchDocument(long id,
        string category,
        DateTime startingTime,
        string matchName,
        string home,
        string away,
        string status)
    {
        Id = id;
        Category = category;
        StartingTime = startingTime;
        MatchName = matchName;
        Home = home;
        Away = away;
        Status = status;
    }

    public string Category { get; }
    public DateTime StartingTime { get; }

    public string MatchName { get; }

    public string Home { get; }
    public string Away { get; }

    public string Status { get; }

    public IEnumerable<MarketDocument> Markets { get; internal set; }
    public long Id { get; }
}
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
        string status,
        long version,
        string modifiedBy = null)
    {
        Id = id;
        Category = category;
        StartingTime = startingTime;
        MatchName = matchName;
        Home = home;
        Away = away;
        Status = status;
        LastModified = DateTime.UtcNow;
        Version = version;
        ModifiedBy = modifiedBy;
    }

    public string Category { get; }
    public DateTime StartingTime { get; }

    public string MatchName { get; }

    public string Home { get; }
    public string Away { get; }

    public string Status { get; }

    public IEnumerable<MarketDocument> Markets { get; internal set; }

    public DateTime LastModified { get; }
    public string ModifiedBy { get; }
    public long Version { get; }
    public long Id { get; }
}
using Shared.Mongo;

namespace Event.Infrastructure.Mongo;

public class MatchDocument : IIdentifiable<long>, IMongoDocument
{
    private DateTime _lastModified;
    private int? _modifiedBy;

    public MatchDocument(long id,
        string category,
        DateTime startingTime,
        string matchName,
        string home,
        string away,
        string status,
        long version,
        int modifiedBy = 0)
    {
        Id = id;
        Category = category;
        StartingTime = startingTime;
        MatchName = matchName;
        Home = home;
        Away = away;
        Status = status;
        LastModified = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }

    public string Category { get; }
    public DateTime StartingTime { get; }

    public string MatchName { get; }

    public string Home { get; }
    public string Away { get; }

    public string Status { get; }

    public IEnumerable<MarketDocument> Markets { get; internal set; }
    public long Id { get; }

    public DateTime LastModified { get; set; }
    public int? ModifiedBy { get; set; }
    public long Version { get; set; }
}
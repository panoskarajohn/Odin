using Shared.Mongo;

namespace Event.Match.Mongo.Document;

public class MatchDocument : IIdentifiable<long>
{
    public long Id { get; }
    public string Category { get;  }
    public DateTime StartingTime { get; }

    public MatchDocument(long id, string category, DateTime startingTime)
    {
        Id = id;
        Category = category;
        StartingTime = startingTime;
    }
}
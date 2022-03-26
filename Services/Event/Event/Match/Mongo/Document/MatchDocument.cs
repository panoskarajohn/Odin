﻿using Shared.Mongo;

namespace Event.Match.Mongo.Document;

public class MatchDocument : IIdentifiable<long>
{
    public long Id { get; }
    public string Category { get;  }
    public DateTime StartingTime { get; }

    public string MatchName { get; }

    public string Home { get; }
    public string Away { get;  }

    public MatchDocument(long id, string category, DateTime startingTime, string matchName, string home, string away)
    {
        Id = id;
        Category = category;
        StartingTime = startingTime;
        MatchName = matchName;
        Home = home;
        Away = away;
    }
}
﻿using Event.ValueObjects;
using Shared.Domain;
using Shared.IdGenerator;

namespace Event.Match.Models;

public class Match : BaseAggregateRoot<long>
{
    private Match()
    {
        
    }

    public static Match Create(Category category,StartingTime startingTime,string home, string away,long? id = null)
    {
        return new()
        {
            Id = id ?? SnowFlakIdGenerator.NewId(),
            Category = category,
            StartingTime = startingTime,
            MatchName = new MatchName(home, away)
        };
    }

    public Category Category { get; private set; }
    public StartingTime StartingTime { get; private set; }
    public MatchName MatchName { get; private set; }
    
    
}
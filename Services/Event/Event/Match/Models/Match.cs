using Event.ValueObjects;
using Shared.Domain;
using Shared.IdGenerator;

namespace Event.Match.Models;

public class Match : BaseAggregateRoot<long>
{
    private Match()
    {
        
    }

    public static Match Create(Category category,StartingTime startingTime,long? id = null)
    {
        return new()
        {
            Id = id ?? SnowFlakIdGenerator.NewId(),
            Category = category,
            StartingTime = startingTime
        };
    }

    public Category Category { get; private set; }
    public StartingTime StartingTime { get; private set; }
}
using Event.Core.Enumerations;
using Event.Core.ValueObjects;
using Shared.Domain;
using Shared.IdGenerator;

namespace Event.Core.Models;

public class Match : BaseAggregateRoot<long>
{
    private Match()
    {
    }

    public Category Category { get; private set; }
    public StartingTime StartingTime { get; private set; }
    public MatchName MatchName { get; private set; }
    public Status Status { get; private set; }

    public static Match Create(Category category, StartingTime startingTime, string home, string away,
        string status = nameof(Status.Active), long? id = null)
    {
        return new()
        {
            Id = id ?? SnowFlakIdGenerator.NewId(),
            Category = category,
            StartingTime = startingTime,
            MatchName = new MatchName(home, away),
            Status = Status.FromName(status)
        };
    }

    public void Suspend()
    {
        this.Status = Status.Suspended;
    }

    public void Complete()
    {
        this.Status = Status.Completed;
    }
}
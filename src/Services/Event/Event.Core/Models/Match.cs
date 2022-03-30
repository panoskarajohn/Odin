using Event.Core.Enumerations;
using Event.Core.Exceptions;
using Event.Core.ValueObjects;
using Shared.Domain;
using Shared.IdGenerator;

namespace Event.Core.Models;

public class Match : BaseAggregateRoot<long>
{
    private readonly ISet<Market> _markets = new HashSet<Market>();

    private Match()
    {
    }

    public Category Category { get; private set; }
    public StartingTime StartingTime { get; private set; }
    public MatchName MatchName { get; private set; }
    public Status Status { get; private set; }
    public IEnumerable<Market> Markets => _markets;

    public static Match Create(Category category, StartingTime startingTime, string home, string away,
        string status = nameof(Status.Active), long? id = null)
    {
        return new()
        {
            Id = id ?? SnowFlakIdGenerator.NewId(),
            Category = category,
            StartingTime = startingTime,
            MatchName = new(home, away),
            Status = Status.FromName(status)
        };
    }

    /// <summary>
    /// Appends markets to matches
    /// </summary>
    /// <param name="markets"></param>
    /// <exception cref="NullMarketException"></exception>
    public void AppendMarkets(IEnumerable<Market> markets)
    {
        if (markets is null) throw new NullMarketException();

        foreach (var market in markets)
        {
            AppendMarket(market);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="market"></param>
    /// <exception cref="NullMarketException"></exception>
    /// <exception cref="DuplicateMarketException"></exception>
    private void AppendMarket(Market market)
    {
        if (market is null) throw new NullMarketException();
        if (_markets.Contains(market)) throw new DuplicateMarketException(market.Name);

        _markets.Add(market);
    }

    /// <summary>
    /// Suspends a match
    /// </summary>
    public void Suspend()
    {
        this.Status = Status.Suspended;
    }

    /// <summary>
    /// Completes a match
    /// </summary>
    public void Complete()
    {
        this.Status = Status.Completed;
    }
}
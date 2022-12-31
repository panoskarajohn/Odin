using Event.Core.ValueObjects;
using Shared.Domain;
using Shared.IdGenerator;

namespace Event.Core.Models;

public class MarketTemplate : BaseAggregateRoot<long>
{
    public MarketName MarketName { get; }
    public Category Category { get; }
    public StakeLimits StakeLimits { get; private set; }
    
    private MarketTemplate(MarketName marketName, Category category, long? id = null)
    {
        Id = id ?? SnowFlakIdGenerator.NewId();
        MarketName = marketName;
        Category = category;
        CreatedAt = DateTime.UtcNow;
    }
    
    public static MarketTemplate Create(MarketName marketName, Category category, long? id = null)
    {
        return new(marketName, category, id);
    }
    
    public MarketTemplate WithStakeLimits(decimal minStake, decimal maxStake)
    {
        StakeLimits = StakeLimits.Create(minStake, maxStake);
        return this;
    }
}
using Shared.Mongo;

namespace Event.Infrastructure.Mongo;

public class MarketTemplateDocument : IIdentifiable<long>
{
    public MarketTemplateDocument(long id, string marketName, string category, decimal minStake, decimal maxStake, long version)
    {
        Id = id;
        MarketName = marketName;
        Category = category;
        MinStake = minStake;
        MaxStake = maxStake;
        Version = version;
    }
    
    public long Id { get; }
    public string MarketName { get; }
    public string Category { get; }
    public decimal MinStake { get; }
    public decimal MaxStake { get; }
    public long Version { get; }
    
}

using Event.Core.Models;
using Event.Infrastructure.Mongo;

namespace Event.Infrastructure.Mapping;

public static class MarketTemplateToDocument
{
    public static MarketTemplateDocument ToDocument(this MarketTemplate model)
    {
        return new MarketTemplateDocument(
            model.Id, 
            model.MarketName, 
            model.Category, 
            model.StakeLimits.MinStake,
            model.StakeLimits.MaxStake,
            model.Version);
    }
}
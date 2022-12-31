using Event.Core.Models;
using Event.Infrastructure.Mongo;

namespace Event.Infrastructure.Mapping.ToEntity;

public static class MarketTemplateToEntity
{
    public static MarketTemplate ToEntity(this MarketTemplateDocument document)
    {
        return MarketTemplate
            .Create(document.MarketName, document.Category, document.Id)
            .WithStakeLimits(document.MinStake, document.MaxStake);
    }
}
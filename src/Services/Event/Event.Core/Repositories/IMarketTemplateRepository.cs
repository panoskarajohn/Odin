using Event.Core.Models;

namespace Event.Core.Repositories;

public interface IMarketTemplateRepository
{
    public Task<MarketTemplate> Get(long id);
    public Task<MarketTemplate?> Get(string marketName, string category);
    public Task Add(MarketTemplate marketTemplate);
    public Task Update(MarketTemplate marketTemplate);
}
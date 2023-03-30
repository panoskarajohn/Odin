using Event.Core.Models;

namespace Event.Core.Repositories;

public interface IMatchRepository
{
    public Task<Match?> Get(long id);
    public Task Add(Match match, string userId);
    public Task Update(Match match, string userId);
}
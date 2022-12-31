namespace Event.Core.Repositories;

public interface IMatchRepository
{
    public Task<Models.Match?> Get(long id);
    public Task Add(Models.Match match, string userId);
    public Task Update(Models.Match match, string userId);
}
namespace Event.Match.Repositories;

public interface IMatchRepository
{
    public Task<Models.Match> Get(long id);
    public Task Add(Models.Match match);
    public Task Update(Models.Match match);
}
using Backoffice.Models;

namespace Backoffice.ApiServices;

public class EventService : IEventService
{

    public EventService()
    {
    }

    public async Task<Event> GetEventAsync(int id)
    {
        throw new NotImplementedException();
    }
}
using Backoffice.Models;

namespace Backoffice.ApiServices;

public class EventService : IEventService
{
    private readonly HttpClient _httpClient;

    public EventService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Event> GetEventAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Event>($"api/events/{id}");
    }
}
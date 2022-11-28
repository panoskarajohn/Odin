using Ardalis.GuardClauses;
using Backoffice.Models;
using Newtonsoft.Json;

namespace Backoffice.ApiServices;

public class EventService : IEventService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EventService> _logger;

    public EventService(HttpClient httpClient, ILogger<EventService> logger)
    {
        _httpClient = Guard.Against.Null(httpClient);
        _logger = Guard.Against.Null(logger);
    }

    public async Task<Event?> GetEventAsync(long id)
    {
        var response = await _httpClient.GetAsync($"/api/v1/match/{id}");
        
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"Error getting event {id} from API: {response.StatusCode}");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Event>(content);
    }
}
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Slip.Grpc;

namespace Slip.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly Event.EventClient _client;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, Event.EventClient client)
    {
        _logger = logger;
        _client = client;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async  Task<IEnumerable<WeatherForecast>> Get()
    {
        
        var eventResponse = await _client.GetEventAsync(new GetEventRequest() 
            {Id = 6832495717777408});
        
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = eventResponse.Name
            })
            .ToArray();
    }
}
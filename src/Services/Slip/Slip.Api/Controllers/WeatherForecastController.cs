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

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async  Task<IEnumerable<WeatherForecast>> Get()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:3001");
        var client = new Event.EventClient(channel);
        var eventResponse = await client.GetEventAsync(new GetEventRequest() 
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
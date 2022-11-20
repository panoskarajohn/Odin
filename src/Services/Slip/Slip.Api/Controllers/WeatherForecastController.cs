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
    private readonly IConfiguration _configuration;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, Event.EventClient client, IConfiguration configuration)
    {
        _logger = logger;
        _client = client;
        _configuration = configuration;
    }

    [HttpGet("{id}")]
    public async  Task<IEnumerable<WeatherForecast>> Get([FromRoute] long id)
    {
        _logger.LogInformation($"The url to hit is {_configuration.GetValue<string>("EventGrpcUrl")} and Id: {id}");
        var eventResponse = await _client.GetEventAsync(new GetEventRequest() 
            {Id = id});
        
        
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = eventResponse.Name
            })
            .ToArray();
    }
}
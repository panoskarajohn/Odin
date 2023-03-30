using Shared.Cqrs;
using Shared.Jwt;
using Shared.Logging;
using Shared.Metrics;
using Shared.Prometheus;
using Shared.RabbitMq;
using Shared.Swagger;
using Shared.Web;
using Slip.Application;
using Slip.Application.Commands.BuildSlip;
using Slip.Application.Commands.PlaceBet;
using Slip.Grpc;

var builder = WebApplication.CreateBuilder(args);
var host = builder.Host;
var env = builder.Environment;
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddAuth(configuration)
    .AddApplication(configuration)
    .AddErrorHandling()
    .AddMetrics(configuration)
    .AddPrometheus(configuration)
    .AddSwaggerDocs(configuration)
    .AddRabbitMq(configuration)
    .AddCustomVersioning();

builder.Services.AddSlipApplication(configuration);

builder.Services.AddGrpcClient<Event.EventClient>(options =>
{
    options.Address = new Uri(configuration["urls:EventGrpcUrl"]);
});

host.UseLogging();

var app = builder.Build();

app
    .UseApplication()
    .UseAuthorization()
    .UseAuthorization()
    .UseErrorHandling()
    .UseLogging()
    .UseMetrics()
    .UseSwaggerDocs()
    .UsePrometheus()
    .UseRabbitMq();

app.MapGet("/ping", e => e.Response.WriteAsync("pong"))
    .WithName("ping")
    .WithTags("ping");

app.MapPost("/buildSlip",
        async (BuildSlipCommand buildSlipCommand, CancellationToken cancellationToken, IDispatcher dispatcher) =>
        {
            await dispatcher.SendAsync(buildSlipCommand, cancellationToken);
            return Results.NoContent();
        })
    .RequireAuthorization()
    .WithTags("Slip")
    .WithName("Build Slip");

app.MapPost("/placeBet",
        async (CancellationToken cancellationToken, IDispatcher dispatcher) =>
        {
            var placeBetCommand = new PlaceBetCommand();
            await dispatcher.SendAsync(placeBetCommand, cancellationToken);
            return Results.NoContent();
        })
    .RequireAuthorization()
    .WithTags("Slip")
    .WithName("Place Bet");

app.MapControllers();

app.Run();
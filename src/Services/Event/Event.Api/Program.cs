using System.Net;
using Event.Application;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Shared.IdGenerator;
using Shared.Logging;
using Shared.Metrics;
using Shared.Prometheus;
using Shared.Swagger;
using Shared.Web;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var env = builder.Environment;
var host = builder.Host;
var webHost = builder.WebHost;

//
SnowFlakIdGenerator.Configure(1);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddApplication(configuration)
    .AddErrorHandling()
    .AddMetrics(configuration)
    .AddPrometheus(configuration)
    .AddCustomSwagger(configuration, typeof(IEventMarker).Assembly)
    .AddCustomVersioning();

//Register Event
builder.Services.AddEventApplication(configuration);

//Decorators
builder.Services.AddLoggingDecorators();

//Integrates serilog to the application
host.UseLogging();

webHost.ConfigureKestrel(options =>
{
    var ports = GetDefinedPorts(configuration);

    (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
    {
        var grpcPort = config.GetValue("GRPC_PORT", 81);
        var port = config.GetValue("PORT", 80);
        return (port, grpcPort);
    }

    options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
    options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseApplication()
    .UseErrorHandling()
    .UseLogging()
    .UseMetrics()
    .UsePrometheus();

if (env.IsDevelopment())
{
    var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
    app.UseCustomSwagger(provider);
}

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", e => e.Response.WriteAsync("Hello from Event.Api"));

app.Run();
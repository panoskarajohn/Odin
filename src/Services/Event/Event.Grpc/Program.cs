using System.Net;
using Event.Application;
using Event.Grpc.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared.Grpc;
using Shared.Logging;
using Shared.Metrics;
using Shared.Prometheus;
using Shared.Web;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var host = builder.Host;
var configuration = builder.Configuration;
var webHost = builder.WebHost;

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services
    .AddApplication(configuration)
    .AddErrorHandling()
    .AddMetrics(configuration)
    .AddPrometheus(configuration)
    .AddCustomGrpc();

builder.Services.AddEventApplication(configuration);
webHost.ConfigureKestrel(options =>
{
    var ports = GetDefinedPorts(configuration);

    (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
    {
        var grpcPort = config.GetValue("GRPC_PORT", 4081);
        var port = config.GetValue("PORT", 4000);
        return (port, grpcPort);
    }

    options.Listen(IPAddress.Any, ports.httpPort,
        listenOptions => { listenOptions.Protocols = HttpProtocols.Http1AndHttp2; });
    options.Listen(IPAddress.Any, ports.grpcPort, listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
});
host.UseLogging();

var app = builder.Build();

app.MapGrpcService<EventGrpcService>();
app.MapGrpcHealthChecksService();

app
    .UseApplication()
    .UseLogging()
    .UseErrorHandling()
    .UseMetrics()
    .UsePrometheus();

app.Run();
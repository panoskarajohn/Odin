using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Shared.Jwt;
using Shared.Logging;
using Shared.Metrics;
using Shared.Prometheus;
using Shared.Swagger;
using Shared.Web;
using Slip.Application;
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
    .AddCustomSwagger(configuration, typeof(ISlipMarker).Assembly)
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
    .UsePrometheus();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
    app.UseCustomSwagger(provider);
}

app.MapControllers();

app.Run();
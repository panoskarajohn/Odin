using System.Net;
using Event.Application;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
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

//
SnowFlakIdGenerator.Configure(1);

// Add services to the container.
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:5000";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new()
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
    {
        policy.RequireClaim("client_id","eventClient");
    });
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", e => e.Response.WriteAsync("Hello from Event.Api"));

app.Run();
using Identity.Core;
using Identity.Core.Commands;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Shared.Cqrs;
using Shared.IdGenerator;
using Shared.Logging;
using Shared.Metrics;
using Shared.Prometheus;
using Shared.Swagger;
using Shared.Web;
using Shared.Security;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var env = builder.Environment;
var host = builder.Host;

// Add services to the container.
SnowFlakIdGenerator.Configure(1);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddApplication(configuration)
    .AddErrorHandling()
    .AddOdinSecurity(configuration)
    .AddMetrics(configuration)
    .AddPrometheus(configuration)
    .AddCustomSwagger(configuration, typeof(Program).Assembly)
    .AddCustomVersioning();

builder.Services.AddIdentityApplication(configuration);

host.UseLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
    app.UseCustomSwagger(provider);
}

app
    .UseApplication()
    .UseErrorHandling()
    .UseLogging()
    .UseMetrics()
    .UsePrometheus();

app.MapControllers();
app.MapGet("/", e => e.Response.WriteAsync("Hello from Identity.Api"));
app.MapPost("/sign-up", async (SignUp command, IDispatcher dispatcher) =>
{
    await dispatcher.SendAsync(command with {UserId = Guid.NewGuid()});
    return Results.NoContent();
}).WithTags("Account").WithName("Sign up");

app.Run();
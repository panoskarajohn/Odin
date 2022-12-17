using Identity.Core.Commands;
using Shared.Cqrs;
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

// Add services to the container.
SnowFlakIdGenerator.Configure(1);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddApplication(configuration)
    .AddErrorHandling()
    .AddMetrics(configuration)
    .AddPrometheus(configuration)
    .AddCustomVersioning();

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

app.MapPost("/sign-up", async (SignUp command, IDispatcher dispatcher) =>
{
    await dispatcher.SendAsync(command with {UserId = Guid.NewGuid()});
    return Results.NoContent();
}).WithTags("Account").WithName("Sign up");



app.MapControllers();

app.Run();
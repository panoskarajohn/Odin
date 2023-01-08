using Identity.Core;
using Identity.Core.Commands;
using Identity.Core.Queries;
using Identity.Core.Services;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Shared.Cqrs;
using Shared.IdGenerator;
using Shared.Jwt;
using Shared.Logging;
using Shared.Metrics;
using Shared.Prometheus;
using Shared.Swagger;
using Shared.Web;
using System.Text;
using Identity.Core.Entities;
using Newtonsoft.Json;
using Shared.Security;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var env = builder.Environment;
var host = builder.Host;

configuration.AddEnvironmentVariables();
// Add services to the container.
SnowFlakIdGenerator.Configure(1);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddAuth(configuration)
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


app
    .UseApplication()
    .UseAuthentication()
    .UseAuthorization()
    .UseErrorHandling()
    .UseLogging()
    .UseMetrics()
    .UsePrometheus();

app.MapGet("/", e => e.Response.WriteAsync("Hello from Identity.Api"));


app.MapPost("/sign-up", async (SignUp command, IDispatcher dispatcher) =>
{
    await dispatcher.SendAsync(command with {UserId = Guid.NewGuid()});
    return Results.NoContent();
}).WithTags("Account").WithDisplayName("Sign up").WithName("Sign up");


app.MapPost("/sign-in", async (SignIn command, IDispatcher dispatcher, ITokenStorage storage) =>
{
    await dispatcher.SendAsync(command);
    var jwt = storage.Get();
    return Results.Ok(jwt);
}).WithTags("Account").WithName("Sign in");


app.MapGet("/me", async (IDispatcher dispatcher, HttpContext context) =>
{
    var user = await dispatcher.QueryAsync(new GetUser {UserId = UserId(context)});
    return user is null ? Results.NotFound() : Results.Ok(user);
}).RequireAuthorization().WithTags("Account").WithName("Get account");


app.MapGet("/users/{id:guid}", async (Guid id, IDispatcher dispatcher) =>
{
    var user = await dispatcher.QueryAsync(new GetUser {UserId = id});
    return user is null ? Results.NotFound() : Results.Ok(user);
}).WithTags("Users").WithName("Get user");


static Guid UserId(HttpContext context)
    => string.IsNullOrWhiteSpace(context.User.Identity?.Name) ? Guid.Empty : Guid.Parse(context.User.Identity.Name);


if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
    app.UseCustomSwagger(provider);
}
app.Run();
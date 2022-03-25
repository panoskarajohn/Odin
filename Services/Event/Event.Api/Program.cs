using Event;
using Serilog;
using Shared.Logging;
using Shared.Web;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var env = builder.Environment;
var host = builder.Host;


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication(builder.Configuration)
    .AddErrorHandling();

builder.Services.AddEvent(configuration);

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
    .UseLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
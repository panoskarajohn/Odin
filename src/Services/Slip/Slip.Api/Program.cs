using Shared.Metrics;
using Shared.Prometheus;
using Shared.Swagger;
using Shared.Web;
using Slip.Grpc;

var builder = WebApplication.CreateBuilder(args);
var host = builder.Host;
var env = builder.Environment;
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication(configuration)
    .AddErrorHandling()
    .AddMetrics()
    .AddPrometheus(configuration)
    .AddCustomSwagger(configuration, typeof(ISlipMarker).Assembly)
    .AddCustomVersioning();

builder.Services.AddGrpcClient<Event.EventClient>(options =>
{
    options.Address = new Uri(configuration["EventGrpcUrl"]);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
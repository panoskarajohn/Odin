using System.Diagnostics;
using Shared.Logging;
using Shared.Web;
using Shared.Web.Extension;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);
var host = builder.Host;
var conf = builder.Configuration;

builder.Services.AddApplication(conf);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(conf.GetSection("yarp"))
    .AddTransforms(transforms =>
    {
        transforms.AddRequestTransform(transform =>
        {
            var activity = Activity.Current;
            var correlationId = Guid.NewGuid().ToString("N");
            transform.ProxyRequest.Headers.AddCorrelationId(correlationId);

            return ValueTask.CompletedTask;
        });
    });

host.UseLogging();

var app = builder.Build();
app
    .UseApplication()
    .UseLogging();

app.MapGet("/", () => "Odin Gateway");
app.MapReverseProxy();

app.Run();
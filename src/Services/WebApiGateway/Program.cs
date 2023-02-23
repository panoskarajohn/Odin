using System.Diagnostics;
using Shared.Logging;
using Shared.Web;
using Shared.Jwt;
using Shared.Web.Extension;
using WebApiGateway;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;


var builder = WebApplication.CreateBuilder(args);
var host = builder.Host;
var services = builder.Services;
var conf = builder.Configuration;

services
    .AddApplication(conf)
    .AddAuth(conf)
    .AddReverseProxy()
    .LoadFromMemory(RouterConfiguration.Routes, ClusterConfiguration.Clusters)
    .AddTransforms(transforms =>
    {
        transforms.AddRequestTransform(transform =>
        {
            var activity = Activity.Current;
            var correlationId = Guid.NewGuid().ToString("N");
            transform.ProxyRequest.Headers.AddCorrelationId(correlationId);

            return ValueTask.CompletedTask;
        });
    });;

host.UseLogging();

var app = builder.Build();
app
    .UseApplication()
    .UseAuthorization()
    .UseLogging();

app.MapGet("/", () => "Odin Gateway");
app.MapReverseProxy();

app.Run();
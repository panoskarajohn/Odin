using Shared.Common;
using Shared.Jwt;
using Shared.Logging;
using Shared.Web;
using Shared.Web.Extension;
using WebApiGateway;
using WebApiGateway.Config;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);
var host = builder.Host;
var services = builder.Services;
var conf = builder.Configuration;

var urlsConfig = conf.GetOptions<UrlsConfig>("urls");
var clusterConfiguration = new ClusterConfiguration(urlsConfig);
var routeConfiguration = new RouterConfiguration();

services
    .AddApplication(conf)
    .AddAuth(conf)
    .AddSingleton(urlsConfig)
    .AddReverseProxy()
    .LoadFromMemory(routeConfiguration.Routes, clusterConfiguration.Clusters)
    .AddTransforms(transforms =>
    {
        transforms.AddRequestTransform(transform =>
        {
            var correlationId = Guid.NewGuid().ToString("N");
            transform.ProxyRequest.Headers.AddCorrelationId(correlationId);

            return ValueTask.CompletedTask;
        });
    });
;

host.UseLogging();

var app = builder.Build();
app
    .UseApplication()
    .UseAuthorization()
    .UseLogging();

app.MapGet("/", () => "Odin Gateway");
app.MapGet("/ping", () => "pong").WithTags("API").WithName("Pong");
app.MapReverseProxy();

app.Run();
using System.Security.Cryptography.X509Certificates;
using Consul;
using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Polly;
using Polly.Extensions.Http;
using Shared.Common;
using Shared.Types.Interfaces;
using Shared.Web.Clock;
using Shared.Web.Context;
using Shared.Web.ErrorHandling;
using Shared.Web.Extension;
using Shared.Web.LoadBalancing;
using Shared.Web.Logging;
using Shared.Web.Options;
using Shared.Web.ServiceDiscovery;

namespace Shared.Web;

public static class Extensions
{
    /// <summary>
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddContext(this IServiceCollection services)
    {
        services.AddSingleton<ContextAccessor>();
        services.AddTransient(sp => sp.GetRequiredService<ContextAccessor>().Context);
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }

    public static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        return services
            .AddScoped<ErrorHandlerMiddleware>()
            .AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>()
            .AddSingleton<IExceptionCompositionRoot, ExceptionCompositionRoot>();
    }

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var appOptions = configuration.GetOptions<AppOptions>("app");
        services
            .AddSingleton(appOptions)
            .AddHttpContextAccessor()
            .AddContext()
            .AddConsul(configuration)
            .AddFabio(configuration)
            .AddSingleton<IUtcClock, UtcUtcClock>();

        services
            .AddHttpClient(configuration)
            .AddConsulHandler()
            .AddFabioHandler();

        var version = appOptions.DisplayVersion ? $" {appOptions.Version}" : string.Empty;
        Console.WriteLine(FiggleFonts.Doom.Render($"{appOptions.Name} v.{version}"));

        return services;
    }

    private static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("consul");
        var options = section.BindOptions<ConsulOptions>();
        services.Configure<ConsulOptions>(section);
        if (!options.Enabled) return services;

        if (string.IsNullOrWhiteSpace(options.Url))
            throw new ArgumentException("Consul URL cannot be empty.", nameof(options.Url));

        services.AddTransient<ConsulHttpHandler>();
        services.AddHostedService<ConsulRegistrationService>();
        services.AddSingleton<IServiceDiscoveryRegistration, DefaultServiceDiscoveryRegistration>();
        services.AddSingleton<IConsulClient>(new ConsulClient(consulConfig =>
        {
            consulConfig.Address = new Uri(options.Url);
        }));

        return services;
    }

    private static IHttpClientBuilder AddConsulHandler(this IHttpClientBuilder builder)
    {
        return builder.AddHttpMessageHandler<ConsulHttpHandler>();
    }


    private static IServiceCollection AddFabio(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("fabio");
        var options = section.BindOptions<FabioOptions>();
        services.Configure<FabioOptions>(section);

        if (!options.Enabled) return services;

        if (string.IsNullOrWhiteSpace(options.Url))
            throw new ArgumentException("Fabio URL cannot be empty.", nameof(options.Url));

        services.AddTransient<FabioHttpHandler>();
        services.AddSingleton<IServiceDiscoveryRegistration, FabioServiceDiscoveryRegistration>();

        return services;
    }

    private static IHttpClientBuilder AddFabioHandler(this IHttpClientBuilder builder)
    {
        return builder.AddHttpMessageHandler<FabioHttpHandler>();
    }

    private static IHttpClientBuilder AddHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var httpClientSection = configuration.GetSection("httpClient");
        var httpClientOptions = httpClientSection.BindOptions<HttpClientOptions>();
        services.Configure<HttpClientOptions>(httpClientSection);

        var consulOptions = configuration.GetSection("consul").BindOptions<ConsulOptions>();
        var fabioOptions = configuration.GetSection("fabio").BindOptions<FabioOptions>();

        var builder = services
            .AddHttpClient(httpClientOptions.Name)
            .AddTransientHttpErrorPolicy(_ => HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(httpClientOptions.Resiliency.Retries, retry =>
                    httpClientOptions.Resiliency.Exponential
                        ? TimeSpan.FromSeconds(Math.Pow(2, retry))
                        : httpClientOptions.Resiliency.RetryInterval ?? TimeSpan.FromSeconds(2)));

        var certificateLocation = httpClientOptions.Certificate?.Location;
        if (httpClientOptions.Certificate is not null && !string.IsNullOrWhiteSpace(certificateLocation))
        {
            var certificate = new X509Certificate2(certificateLocation, httpClientOptions.Certificate.Password);
            builder.ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(certificate);
                return handler;
            });
        }

        if (httpClientOptions.RequestMasking.Enabled)
            builder.Services.Replace(ServiceDescriptor
                .Singleton<IHttpMessageHandlerBuilderFilter, HttpLoggingFilter>());

        if (string.IsNullOrWhiteSpace(httpClientOptions.Type)) return builder;

        return httpClientOptions.Type.ToLowerInvariant() switch
        {
            "consul" => consulOptions.Enabled ? builder.AddConsulHandler() : builder,
            "fabio" => fabioOptions.Enabled ? builder.AddFabioHandler() : builder,
            _ => throw new InvalidOperationException($"Unsupported HTTP client type: '{httpClientOptions.Type}'.")
        };
    }

    public static IServiceCollection AddHostApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var appOptions = configuration.GetOptions<AppOptions>("app");
        services = services
            .AddSingleton(appOptions);
        var version = appOptions.DisplayVersion ? $" {appOptions.Version}" : string.Empty;
        Console.WriteLine(FiggleFonts.Doom.Render($"{appOptions.Name} v.{version}"));

        return services;
    }

    public static void AddCustomVersioning(this IServiceCollection services,
        Action<ApiVersioningOptions> configurator = null)
    {
        //https://www.meziantou.net/versioning-an-asp-net-core-api.htm
        //https://exceptionnotfound.net/overview-of-api-versioning-in-asp-net-core-3-0/
        services.AddApiVersioning(options =>
        {
            // Add the headers "api-supported-versions" and "api-deprecated-versions"
            // This is better for discoverability
            options.ReportApiVersions = true;

            // AssumeDefaultVersionWhenUnspecified should only be enabled when supporting legacy services that did not previously
            // support API versioning. Forcing existing clients to specify an explicit API version for an
            // existing service introduces a breaking change. Conceptually, clients in this situation are
            // bound to some API version of a service, but they don't know what it is and never explicit request it.
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);

            // // Defines how an API version is read from the current HTTP request
            options.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("api-version"),
                new UrlSegmentApiVersionReader());

            configurator?.Invoke(options);
        });
    }

    /// <summary>
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    private static IApplicationBuilder UseContext(this IApplicationBuilder app)
    {
        app.Use((ctx, next) =>
        {
            ctx.RequestServices.GetRequiredService<ContextAccessor>().Context = new Context.Context(ctx);
            return next();
        });
        return app;
    }


    /// <summary>
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseApplication(this IApplicationBuilder app)
    {
        app.UseContext();
        app.UseCorrelationId();

        using var scope = app.ApplicationServices.CreateScope();
        var initializer = scope.ServiceProvider.GetServices<IInitializer>();

        Task.Run(() =>
        {
            foreach (var initializer1 in initializer) initializer1.InitAsync().GetAwaiter().GetResult();
        });

        return app;
    }

    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandlerMiddleware>();
    }
}
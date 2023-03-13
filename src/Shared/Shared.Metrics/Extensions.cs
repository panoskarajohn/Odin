﻿using System.ComponentModel;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Health.Endpoints;
using App.Metrics.AspNetCore.Tracking;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Common;
using Shared.Metrics.Builders;
using Shared.Web.Options;

namespace Shared.Metrics;

public static class Extensions
{
    private const string MetricsSectionName = "metrics";
    private const string AppSectionName = "app";
    private const string RegistryName = "metrics.metrics";
    private static bool _initialized;

    [Description(
        "For the time being it sets Kestrel's AllowSynchronousIO = true, see https://github.com/AppMetrics/AppMetrics/issues/396")]
    public static IServiceCollection AddMetrics(this IServiceCollection services, IConfiguration configuration,
        string metricsSectionName = MetricsSectionName, string appSectionName = AppSectionName)
    {
        if (string.IsNullOrWhiteSpace(metricsSectionName)) metricsSectionName = MetricsSectionName;

        if (string.IsNullOrWhiteSpace(appSectionName)) appSectionName = AppSectionName;

        var metricsOptions = configuration.GetOptions<MetricsOptions>(metricsSectionName);
        var appOptions = configuration.GetOptions<AppOptions>(AppSectionName);

        return services.AddMetrics(configuration, metricsOptions, appOptions);
    }

    [Description(
        "For the time being it sets Kestrel's AllowSynchronousIO = true, see https://github.com/AppMetrics/AppMetrics/issues/396")]
    public static IServiceCollection AddMetrics(this IServiceCollection services, IConfiguration configuration,
        Func<IMetricsOptionsBuilder, IMetricsOptionsBuilder> buildOptions, string appSectionName = AppSectionName)
    {
        if (string.IsNullOrWhiteSpace(appSectionName)) appSectionName = AppSectionName;

        var metricsOptions = buildOptions(new MetricsOptionsBuilder()).Build();
        var appOptions = configuration.GetOptions<AppOptions>(appSectionName);

        return services.AddMetrics(configuration, metricsOptions, appOptions);
    }

    [Description(
        "For the time being it sets Kestrel's and IIS ServerOptions AllowSynchronousIO = true, see https://github.com/AppMetrics/AppMetrics/issues/396")]
    public static IServiceCollection AddMetrics(this IServiceCollection services, IConfiguration configuration,
        MetricsOptions metricsOptions,
        AppOptions appOptions)
    {
        services.AddSingleton(metricsOptions);

        _initialized = true;

        //TODO: Remove once fixed https://github.com/AppMetrics/AppMetrics/issues/396
        services.Configure<KestrelServerOptions>(o => o.AllowSynchronousIO = true);
        services.Configure<IISServerOptions>(o => o.AllowSynchronousIO = true);

        var metricsBuilder = new MetricsBuilder().Configuration.Configure(cfg =>
        {
            var tags = metricsOptions.Tags;
            if (tags is null) return;

            tags.TryGetValue("app", out var app);
            tags.TryGetValue("env", out var env);
            tags.TryGetValue("server", out var server);
            cfg.AddAppTag(string.IsNullOrWhiteSpace(app) ? appOptions.Service : app);
            cfg.AddEnvTag(string.IsNullOrWhiteSpace(env) ? null : env);
            cfg.AddServerTag(string.IsNullOrWhiteSpace(server) ? null : server);
            if (!string.IsNullOrWhiteSpace(appOptions.Instance)) cfg.GlobalTags.Add("instance", appOptions.Instance);

            if (!string.IsNullOrWhiteSpace(appOptions.Version)) cfg.GlobalTags.Add("version", appOptions.Version);

            foreach (var tag in tags)
            {
                if (cfg.GlobalTags.ContainsKey(tag.Key)) cfg.GlobalTags.Remove(tag.Key);

                if (!cfg.GlobalTags.ContainsKey(tag.Key)) cfg.GlobalTags.TryAdd(tag.Key, tag.Value);
            }
        });

        if (metricsOptions.InfluxEnabled)
            metricsBuilder.Report.ToInfluxDb(o =>
            {
                o.InfluxDb.Database = metricsOptions.Database;
                o.InfluxDb.BaseUri = new Uri(metricsOptions.InfluxUrl);
                o.InfluxDb.CreateDataBaseIfNotExists = true;
                o.FlushInterval = TimeSpan.FromSeconds(metricsOptions.Interval);
            });

        var metrics = metricsBuilder.Build();
        var metricsWebHostOptions = GetMetricsWebHostOptions(metricsOptions);

        using var serviceProvider = services.BuildServiceProvider();

        services.AddHealth();
        services.AddHealthEndpoints(configuration);
        services.AddMetricsTrackingMiddleware(configuration);
        services.AddMetricsEndpoints(configuration);
        services.AddSingleton<IStartupFilter>(new DefaultMetricsEndpointsStartupFilter());
        services.AddSingleton<IStartupFilter>(new DefaultHealthEndpointsStartupFilter());
        services.AddSingleton<IStartupFilter>(new DefaultMetricsTrackingStartupFilter());
        services.AddMetricsReportingHostedService(metricsWebHostOptions.UnobservedTaskExceptionHandler);
        services.AddMetricsEndpoints(metricsWebHostOptions.EndpointOptions, configuration);
        services.AddMetricsTrackingMiddleware(metricsWebHostOptions.TrackingMiddlewareOptions, configuration);
        services.AddMetrics(metrics);

        return services;
    }

    private static MetricsWebHostOptions GetMetricsWebHostOptions(MetricsOptions metricsOptions)
    {
        var options = new MetricsWebHostOptions();

        if (!metricsOptions.Enabled) return options;

        if (!metricsOptions.PrometheusEnabled) return options;

        options.EndpointOptions = endpointOptions =>
        {
            switch (metricsOptions.PrometheusFormatter?.ToLowerInvariant() ?? string.Empty)
            {
                case "protobuf":
                    endpointOptions.MetricsEndpointOutputFormatter =
                        new MetricsPrometheusProtobufOutputFormatter();
                    break;
                default:
                    endpointOptions.MetricsEndpointOutputFormatter =
                        new MetricsPrometheusTextOutputFormatter();
                    break;
            }
        };

        return options;
    }

    public static IApplicationBuilder UseMetrics(this IApplicationBuilder app)
    {
        MetricsOptions options;
        using (var scope = app.ApplicationServices.CreateScope())
        {
            options = scope.ServiceProvider.GetRequiredService<MetricsOptions>();
        }

        return !options.Enabled
            ? app
            : app.UseHealthAllEndpoints()
                .UseMetricsAllEndpoints()
                .UseMetricsAllMiddleware();
    }
}
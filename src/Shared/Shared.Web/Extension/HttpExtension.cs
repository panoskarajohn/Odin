using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Shared.Web.Extension;

public static class HttpContextExtensions
{
    private const string CorrelationIdKey = "correlation-id";

    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        => app.Use(async (ctx, next) =>
        {
            ctx.Items.Add(CorrelationIdKey, Guid.NewGuid());
            await next();
        });

    public static Guid? TryGetCorrelationId(this HttpContext context)
        => context.Items.TryGetValue(CorrelationIdKey, out var id) ? (Guid) id : null;

    public static string GetUserIpAddress(this HttpContext context)
    {
        if (context is null)
        {
            return string.Empty;
        }

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (context.Request.Headers.TryGetValue("x-forwarded-for", out var forwardedFor))
        {
            var ipAddresses = forwardedFor.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (ipAddresses.Any())
            {
                ipAddress = ipAddresses[0];
            }
        }

        return ipAddress ?? string.Empty;
    }

    public static void AddCorrelationId(this HttpRequestHeaders headers, string correlationId)
        => headers.TryAddWithoutValidation(CorrelationIdKey, correlationId);
}